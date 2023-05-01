using Microsoft.AspNetCore.Mvc;
using TelegrammService.service;
using WebApplication1.Controllers;

namespace TelegrammService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TelegrammService : ControllerBase
    {

        //TelegrammSenderService telegrammService = new TelegrammSenderService();
        /*
        [HttpPost(Name = "SendMessage")]
        public Autentification sendMessage(Autentification source)
        {
            return source;
        }
        */
        /*
        [HttpPost(Name = "AutentificationClient")]
        public Autentification autentificationClient(Autentification source)
        {
            // устанавливаем id для нового пользователя
            //int apiId = 21994278;
            //string apiHash = "127649946d135636f95e7b775f3068c7";
            //string phoneNumber = "+79171688704";
           // telegrammService.autentificationClientAsync(source.apiId, source.apiHash, source.phoneNumber);
            return source;
        }*/
        /*
        [HttpPost(Name = "autentificationSetCode")]
        public Autentification autentificationSetCode(Autentification source)
        {
            // устанавливаем id для нового пользователя
            //int apiId = 21994278;
            //string apiHash = "127649946d135636f95e7b775f3068c7";
            //string phoneNumber = "+79171688704";
            telegrammService.autentificationSetCodeAsync(source.apiHash, source.phoneNumber, source.code);
            return source;
        }*/
    }
}