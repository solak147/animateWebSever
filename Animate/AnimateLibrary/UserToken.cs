using System.Net;

namespace AnimateLibrary
{
    public class UserToken
    {
        public HttpStatusCode StatusCode { get; set; }
        public string token { get; set; }
        public DateTime ExpireTime { get; set; }
    }
}