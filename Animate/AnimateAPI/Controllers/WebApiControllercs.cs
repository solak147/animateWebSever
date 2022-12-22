using Microsoft.AspNetCore.Mvc;

namespace AnimateAPI.Controllers
{
    [Route("[controller]/[action]")]
    public class WebApiController : ControllerBase
    {
        /// <summary>
        /// y
        /// </summary>
        /// <param name="type">Model Name</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult FlushStatic(string type)
        {
            return new JsonResult(new { status = true, message = "Twip Update" });
        }
    }

    //https://blog.csdn.net/m0_62355555/article/details/124325578

}
