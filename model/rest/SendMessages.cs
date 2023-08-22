namespace TelegrammService.model
{
    public class SendMessages
    {

        public int apiId { get; set; }

        public List<string> logins { get; set; }
        
        public string message { get; set; } = "";
    }
}
