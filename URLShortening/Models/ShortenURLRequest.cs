using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace URLShortening.Models
{
    //[ApiController]
    //[Route("api/[controller]")]
    //[EnableCors("AllowSpecificOrigin")]
    public class ShortenURLRequest
    {
        public string Url { get; set; }=string.Empty;


    }
}
