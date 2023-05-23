
using TelegrammService.model;
using WTelegram;
using System;
using System.Linq;
using TL;

namespace TelegrammService.service
{
    public class TelegrammSenderService
    {
        private MessageCounterService messageCounterService;
        private static ClientConfig clientConfig;
        private Client client;

        public TelegrammSenderService()
        {
            clientConfig = new ClientConfig();
        }

        public async Task<PostResult> autentificationClientAsync(int apiId, string apiHash, string phoneNumber, string sessionPath)
        {
            try
            {
                clientConfig.apiId = apiId;
                clientConfig.apiHash = apiHash;
                clientConfig.phoneNumber = phoneNumber;
                clientConfig.sessionPath = sessionPath;

                client = new Client(Config);
                var myself = await client.LoginUserIfNeeded();
                Console.WriteLine($"We are logged-in as {myself} (id {myself.id})");

                return PostResultService.getOkPostResult(apiId, "Autentification successfully");
            }
            catch (Exception ex)
            {
                return PostResultService.getErrorPostResult(apiId, ex.Message);
            }
        }

        public async Task<PostResult> sendMessage(int apiId, string login, string message)
        {
            try
            {
                if (clientConfig == null)
                {
                    return PostResultService.getOkPostResult(apiId, "Client not authorized! Use api autentificationClient");
                }
                if (messageCounterService.checkLimitExceeded(apiId))
                {
                    return PostResultService.getErrorPostResult(apiId, "ApiId limit message is exceeded:" + apiId + ", limit:" + messageCounterService.getLimit(apiId));
                }
                var myself = await client.LoginUserIfNeeded();
                var resolved = await client.Contacts_ResolveUsername(login);
                await client.SendMessageAsync(resolved, message);
                return PostResultService.getOkPostResult(apiId, "Message was send from:" + apiId + " to:" + login);
            }
            catch (Exception ex)
            {
                return PostResultService.getErrorPostResult(apiId, ex.Message);
            }
        }

        public void setMessageCounterService(MessageCounterService messageCounterService)
        {
            this.messageCounterService = messageCounterService;
        }

        static string Config(string what)
        {
            switch (what)
            {
                case "api_id": return clientConfig.apiId.ToString();    // "21994278";
                case "api_hash": return clientConfig.apiHash;           // "127649946d135636f95e7b775f3068c7";
                case "phone_number": return clientConfig.phoneNumber;   // "+79171688704";
                case "password": return clientConfig.password2FA;       // if user has enabled 2FA
                case "session_pathname": return clientConfig.sessionPath;
                case "verification_code": 
                    Console.Write("Code: "); return Console.ReadLine();
                case "first_name": return "TODO";                       // if sign-up is required
                case "last_name": return "TODO";                        // if sign-up is required
                default: return null;                                   // let WTelegramClient decide the default config
            }
        }

    }
}