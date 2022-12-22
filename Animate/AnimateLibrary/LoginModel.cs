using System.ComponentModel.DataAnnotations;

namespace AnimateLibrary
{
    public class LoginModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }

        //驗證碼滑動x座標
        public int positionX { get; set; }
    }
}
