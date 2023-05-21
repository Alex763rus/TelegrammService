namespace TelegrammService.model
{
    public class PostResult
    {
        public PostResultCode postResultCode { get; set; }
        
        public long apiId { get; set; }
        public string description { get; set; } = "";
        
    }
}

public enum PostResultCode
{
    OK, ERROR
}
