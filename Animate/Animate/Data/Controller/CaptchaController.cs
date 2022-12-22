using Animate.Data.Service;
using Animate.Data.SliderCaptha;
using AnimateLibrary;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

//參考網站
//https://blog.csdn.net/m0_62355555/article/details/124325578

namespace Animate.Data.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaptchaController : ApiController
    {
        private IAuthService authService { get; set; }

        public CaptchaController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpGet]
        public JsonResult GetCaptcha()
        {
            Captcha64Model model = Captcha.GenerateBase64();
            CacheHelper.remove("sliderX");
            CacheHelper.Add("sliderX", model.X);

            return new JsonResult(new
            {
                background = model.Background,
                slider = model.Slide,
                sliderXXXXX = model.X
            });
        }

        /// &lt;summary&gt;
        /// 检查验证
        /// &lt;/summary&gt;
        /// &lt;param name="x"&gt;&lt;/param&gt;
        /// &lt;returns&gt;&lt;/returns&gt;
        [HttpPost]
        public async Task<JsonResult> CheckCaptchaAsync(LoginModel loginModel)
        {
            string Mess = "";
            int Code = 0;
            var session = CacheHelper.Get<int>("sliderX");
            if (session == null)
            {
                Mess = "请刷新验证码";
                Code = 500;
                goto block;
            }

            int sliderX = session;
            int difX = sliderX - loginModel.positionX;
            if (difX >= 0 - Config.blod && difX <= Config.blod)
            {
                Mess = "成功";
                Code = 200;
            }
            else
            {
                Mess = "错误";
                Code = 500;
            }

        block:
            return new JsonResult(new
            {
                Mess,
                Code,
            });
        }
    }
}


