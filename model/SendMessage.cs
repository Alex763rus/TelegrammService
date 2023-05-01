namespace TelegrammService.model
{
    public class SendMessage
    {

        public int apiId { get; set; }

        public int chatId { get; set; }
        
        public string message { get; set; } = "";
    }
}
