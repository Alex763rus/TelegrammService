using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegrammService.model;
using TeleSharp.TL;
using TeleSharp.TL.Messages;
using TLSharp.Core;

namespace TelegrammService.service
{
    public class TelegrammSenderService
    {
        private Dictionary<int, TelegramClient> clientDictionary;
        private int tmpApiId = 0;
        private string tmpHash = "";
        private TelegramClient tmpTelegramClient;

        public TelegrammSenderService() {
            clientDictionary = new Dictionary<int, TelegramClient>();
        }

        public async Task<PostResult> autentificationClientAsync(int apiId, string apiHash, string phoneNumber)
        {
            if (clientDictionary.ContainsKey(apiId))
            {
                var client = clientDictionary[apiId];
                if (!client.IsUserAuthorized())
                {
                    clientDictionary.Remove(apiId);
                }
                else
                {
                    return PostResultService.getOkPostResult(apiId, "Client already exists and authorized");
                }
            }
            try
            {
                tmpApiId = apiId;
                tmpTelegramClient = new TelegramClient(apiId, apiHash);
                await tmpTelegramClient.ConnectAsync();
                tmpHash = await tmpTelegramClient.SendCodeRequestAsync(phoneNumber);
                return PostResultService.getOkPostResult(apiId, "Code sent successfully");
            }
            catch (Exception ex)
            {
                return PostResultService.getErrorPostResult(apiId, ex.Message);
            }
        }

        public async Task<PostResult> autentificationSetCodeAsync(int apiId, string phoneNumber, string code)
        {
            try
            {
                if(tmpTelegramClient == null)
                {
                    return PostResultService.getErrorPostResult(apiId, "TelegramClient is null! Use autentificationClient");
                }
                string? tmpcode = code;
                await tmpTelegramClient.MakeAuthAsync(phoneNumber, tmpHash, tmpcode);
                clientDictionary.Add(tmpApiId, tmpTelegramClient);
                return PostResultService.getOkPostResult(apiId, "Client was added");
            }
            catch (Exception ex)
            {
                return PostResultService.getErrorPostResult(apiId, ex.Message);
            }
        }


        public async Task<PostResult> sendMessage(int apiId, int chatId,  string message)
        {
            try
            {
                if (!clientDictionary.ContainsKey(apiId))
                {
                    return PostResultService.getErrorPostResult(apiId, "Client with apiId not a found! Use api autentificationClient");
                }
                var client = clientDictionary[apiId];
                if (!client.IsUserAuthorized())
                {
                    return PostResultService.getOkPostResult(apiId, "Client not authorized! Use api autentificationClient");
                }
                var contacts = await client.GetContactsAsync();
                var user = contacts.Users
                    .Where(x => x.GetType() == typeof(TLUser))
                    .Cast<TLUser>()
                    .FirstOrDefault(x => x.Id == chatId);
                if(user == null)
                {
                    return PostResultService.getErrorPostResult(apiId, "ChatId not found in contact for api. " + chatId);
                }
                await client.SendMessageAsync(new TLInputPeerUser() { UserId = user.Id }, message);
                return PostResultService.getOkPostResult(apiId, "Message was send to:" + chatId);
            }
            catch (Exception ex)
            {
                return PostResultService.getErrorPostResult(apiId, ex.Message);
            }
        }

    }
}