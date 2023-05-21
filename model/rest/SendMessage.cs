namespace TelegrammService.model
{
    public class SendMessage
    {

        public int apiId { get; set; }

        public long chatId { get; set; }
        
        public string message { get; set; } = "";
    }
}
