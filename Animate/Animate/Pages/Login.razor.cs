using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

namespace Animate.Pages
{
    public partial class Login 
    {
        public class LoginModel
        {
            [DataType(DataType.EmailAddress)]
            [Required(ErrorMessage = "Email is required")]
            public string? Email { get; set; }

           
            [DataType(DataType.Password)]
            [Required(ErrorMessage = "Password is required")]
            public string? Password { get; set; }
        }

        private LoginModel loginModel = new();

        protected override async Task OnParametersSetAsync()
        {

            base.OnParametersSetAsync();
        }

        private void SubmitHandler()
        {
            Console.WriteLine($"{loginModel.Email} {loginModel.Password}");
        }

        private void InvalidHandler()
        {
            Console.WriteLine($"{loginModel.Email} {loginModel.Password}");
        }

    }
}
