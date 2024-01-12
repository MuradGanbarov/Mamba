using System.ComponentModel.DataAnnotations;

namespace Mamba.Areas.MambaAdmin.ViewModels.Account
{
    public class LoginVM
    {
        [Required(ErrorMessage ="Username or email address is required")]
        public string UserNameOrEmail { get; set; }
        [Required(ErrorMessage ="Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsRemember { get; set; }
    }
}
