using System.ComponentModel.DataAnnotations;

namespace AnimateLibrary
{
    public class UserInfo
    {
        [Required(ErrorMessage = "請填寫Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "請填寫Password")]
        public string Password { get; set; }
    }
}