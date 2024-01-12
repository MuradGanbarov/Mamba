using Mamba.Utilities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Mamba.Areas.MambaAdmin.ViewModels.Account
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "You need input name")]
        [MaxLength(25, ErrorMessage = "Name can contain maximum 25 characters")]
        public string Name { get; set; }
        [Required(ErrorMessage ="You need input surname")]
        [MaxLength(25,ErrorMessage ="Surname can contain maximum 25 characters")]
        public string Surname { get; set; }
        [Required(ErrorMessage ="You need input username")]
        public string UserName { get; set; }
        [Required(ErrorMessage ="You need input email address")]
        public string Email { get; set; }
        [Required(ErrorMessage ="You need select gender")]
        public Gender Gender { get; set; }
        [Required(ErrorMessage="Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage ="You need confirm password")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password),ErrorMessage ="Password should be match")]
        public string ConfirmPassword { get; set; }


    }
}
