using System.ComponentModel.DataAnnotations;

namespace Mamba.Areas.MambaAdmin.ViewModels.Settings
{
    public class CreateSettingVM
    {
        [Required]
        [MaxLength(25,ErrorMessage ="Key can contain maximum 25 characters")]
        public string Key { get; set; }
        [Required]
        [MaxLength(25,ErrorMessage ="Value can contain maximum 25 characters")]
        public string Value { get; set; }
    }
}
