namespace TelegrammService.model
{
    public class SendMessage
    {

        public int apiId { get; set; }

        public string login { get; set; }
        
        public string message { get; set; } = "";
    }
}
