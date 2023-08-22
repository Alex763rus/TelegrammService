
using TelegrammService.model;
using WTelegram;
using System;
using System.Linq;
using TL;
using System.Text;

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

        public async Task<PostResult> autentificationClientAsync(int apiId, string apiHash, string phoneNumber, string sessionPath, string password2FA)
        {
            try
            {
                clientConfig.apiId = apiId;
                clientConfig.apiHash = apiHash;
                clientConfig.phoneNumber = phoneNumber;
                clientConfig.sessionPath = sessionPath;
                clientConfig.password2FA = password2FA;

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
        public async Task<PostResult> checkSubscriber(int apiId, int channelId)
        {
            try
            {
                if (clientConfig == null)
                {
                    return PostResultService.getOkPostResult(apiId, "Client not authorized! Use api autentificationClient");
                }
                var myself = await client.LoginUserIfNeeded();
                var chats = await client.Messages_GetAllChats();
                foreach (var (id, chat) in chats.chats)
                {
                    if (id == channelId)
                    {
                        return PostResultService.getOkPostResult(apiId, "true");
                    }
                }
                return PostResultService.getOkPostResult(apiId, "false");
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

        public async Task<PostResult> sendMessages(int apiId, List<string> logins, string message)
        {
            try
            {
                if (clientConfig == null || client == null)
                {
                    return PostResultService.getOkPostResult(apiId, "Client not authorized! Use api autentificationClient");
                }
                if (messageCounterService.checkLimitExceeded(apiId))
                {
                    return PostResultService.getErrorPostResult(apiId, "ApiId limit message is exceeded:" + apiId + ", limit:" + messageCounterService.getLimit(apiId));
                }
                var myself = await client.LoginUserIfNeeded();
                StringBuilder errorMessage = new StringBuilder();
                int countSuccessSendMessages = 0;
                foreach (var login in logins)
                {
                    try
                    {
                        var resolved = await client.Contacts_ResolveUsername(login);
                        await client.SendMessageAsync(resolved, message);
                        ++countSuccessSendMessages;
                    }
                    catch (Exception ex)
                    {
                        errorMessage.Append(login).Append(" : ").Append(ex.Message).AppendLine("");
                    }
                }
                StringBuilder answer = new StringBuilder();
                answer.Append("Всего сообщений: ").AppendLine(logins.Count.ToString())
                .Append("Успешно отправлено: ").AppendLine(countSuccessSendMessages.ToString())
                .Append("Не отправлено: ").AppendLine((logins.Count - countSuccessSendMessages).ToString());
                if (errorMessage.Length == 0)
                {
                    return PostResultService.getOkPostResult(apiId, answer.ToString());
                }
                else
                {
                    answer.AppendLine("Ошибки: ").AppendLine(errorMessage.ToString());
                    return PostResultService.getErrorPostResult(apiId, answer.ToString());
                }
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
                case "api_id": return clientConfig.apiId.ToString();
                case "api_hash": return clientConfig.apiHash;
                case "phone_number": return clientConfig.phoneNumber;
                case "password": return clientConfig.password2FA;
                case "session_pathname": return clientConfig.sessionPath;
                case "verification_code": 
                    Console.Write("Code: "); return Console.ReadLine();
                case "first_name": return "TODO";
                case "last_name": return "TODO";
                default: return null;
            }
        }

    }
}