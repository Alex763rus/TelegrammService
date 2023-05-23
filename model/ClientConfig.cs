namespace TelegrammService.model
{
    public class ClientConfig
    {
        public int apiId { get; set; }
        public string apiHash { get; set; } = "";
        public string phoneNumber { get; set; } = "";
        
        public string sessionPath { get; set; } = "";
        public string password2FA { get; set; } = "";


    }
}
