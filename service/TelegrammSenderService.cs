
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
        private Client client;

        public TelegrammSenderService()
        {
        }

        public async Task<PostResult> init(int apiId, string apiHash)
        {
            try
            {
                if (client != null)
                {
                    return PostResultService.getOkPostResult(apiId, "Client already initialized");
                }
                client = new Client(apiId, apiHash);
                return PostResultService.getOkPostResult(apiId, "Client init successfully");
            }
            catch (Exception ex)
            {
                return PostResultService.getErrorPostResult(apiId, ex.Message);
            }
        }

        public async Task<PostResult> doLogin(string loginInfo)
        {
            try
            {
                var answer = await client.Login(loginInfo);
                string answerText = "";
                switch (answer)
                {
                    case "verification_code": answerText = "Введите код подтверждения:"; break;
                    case "password": answerText = "Введите облачный пароль:"; break;
                    case "name": answerText = "John Doe"; break;
                    default: answerText = answer; break;
                }
                if (answerText == null)
                {
                    answerText = "Авторизация выполнена успешно!";
                }
                return PostResultService.getOkPostResult(answerText);
            }
            catch (Exception ex)
            {
                return PostResultService.getErrorPostResult(0, ex.Message);
            }
        }

        public async Task<PostResult> clientReset()
        {
            try
            {
                //Auth_ResetAuthorizations
                //Account_ResetAuthorization
                client.Reset(true, true);
                return PostResultService.getOkPostResult("reset ok:");
            }
            catch (Exception ex)
            {
                return PostResultService.getErrorPostResult(0, ex.Message);
            }
        }
            public async Task<PostResult> checkSubscriber(int apiId, int channelId)
        {
            try
            {
                if(client == null)
                {
                    return PostResultService.getErrorPostResult(apiId, "Требуется авторизация");
                }
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
                if (client == null)
                {
                    return PostResultService.getErrorPostResult(apiId, "Клиент не авторизован! Требуется авторизация");
                }
                if (messageCounterService.checkLimitExceeded(apiId))
                {
                    return PostResultService.getErrorPostResult(apiId, "ApiId limit message is exceeded:" + apiId + ", limit:" + messageCounterService.getLimit(apiId));
                }
                var resolved = await client.Contacts_ResolveUsername(login);
                await client.SendMessageAsync(resolved, message);
                return PostResultService.getOkPostResult(apiId, "Message was send from:" + apiId + " to:" + login);
            }
            catch (Exception ex)
            {
                return PostResultService.getErrorPostResult(apiId, ex.Message);
            }
        }


        private bool hasContact(Messages_Chats messageChats, string login)
        {
            foreach (var (id, chat) in messageChats.chats)
            {
                try
                {
                    if (chat.MainUsername != null && chat.MainUsername.Equals(login))
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return false;
        }

        private bool hasContact(Messages_Dialogs messages_Dialogs, string login)
        {
            foreach (var (id, chat) in messages_Dialogs.users)
            {
                try
                {
                    if (chat.MainUsername != null && chat.MainUsername.Equals(login))
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return false;
        }
        public async Task<PostResult> sendMessages(int apiId, List<string> logins, string message)
        {
            try
            {
                if (client == null)
                {
                    return PostResultService.getErrorPostResult(apiId, "Клиент не авторизован! Требуется авторизация");
                }
                if (messageCounterService.checkLimitExceeded(apiId))
                {
                    return PostResultService.getErrorPostResult(apiId, "ApiId limit message is exceeded:" + apiId + ", limit:" + messageCounterService.getLimit(apiId));
                }
                StringBuilder errorMessage = new StringBuilder();
                var needCheckContact = messageCounterService.needCheckContact(apiId);
                Messages_Chats chats = null;
                Messages_Dialogs contacts = null;
                if (needCheckContact) {
                    chats = await client.Messages_GetAllChats();
                    contacts = await client.Messages_GetAllDialogs();
                }

                int countSuccessSendMessages = 0;
                foreach (var login in logins)
                {
                    try
                    {
                        if(!needCheckContact || hasContact(chats, login) || hasContact(contacts, login))
                        {
                            var resolved = await client.Contacts_ResolveUsername(login);
                            await client.SendMessageAsync(resolved, message);
                            ++countSuccessSendMessages;
                        }
                        else
                        {
                            errorMessage.Append(login).Append(" : ").Append("с чатом, контактом отсутствует диалог").AppendLine("");
                        }
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

    }
}