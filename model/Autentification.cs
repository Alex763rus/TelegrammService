namespace WebApplication1.Controllers
{
    public class Autentification
    {
        public int apiId { get; set; }
        public string apiHash { get; set; } = "";
        public string phoneNumber { get; set; } = "";

        public string code { get; set; } = "";
    }
}
