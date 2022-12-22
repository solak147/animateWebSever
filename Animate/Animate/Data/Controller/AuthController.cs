using AnimateLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web.Http;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace Animate.Data.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ApiController
    {
        private readonly IConfiguration configuration;

        public AuthController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpPost("Login")]
        public JsonResult Login(LoginModel loginModel)
        {
            //驗證方式純為Demo用
            if (loginModel.email == "abc@gmail.com" && loginModel.password == "abc123")
            {
                return BuildToken(loginModel);
            }
            else
            {
                return new JsonResult(new { });
            }

        }

        /// <summary>
        /// 建立Token
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        private JsonResult BuildToken(LoginModel loginModel)
        {
            //記在jwt payload中的聲明，可依專案需求自訂Claim
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, loginModel.email),
                new Claim(ClaimTypes.Role,"admin")
            };

            //取得對稱式加密 JWT Signature 的金鑰
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwt:Key"]));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //設定token有效期限
            DateTime expireTime = DateTime.Now.AddMinutes(30);

            //產生token
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expireTime,
                signingCredentials: credential
                );
            string jwtToken = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);

            //建立UserToken物件後回傳client
            return new JsonResult(new UserToken()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                token = jwtToken,
                ExpireTime = expireTime
            });

        }
    }
}
