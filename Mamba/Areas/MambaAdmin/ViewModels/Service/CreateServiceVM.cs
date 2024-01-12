using System.ComponentModel.DataAnnotations;

namespace Mamba.Areas.MambaAdmin.ViewModels
{
    public class CreateServiceVM
{
    [Required]
    [MaxLength(25, ErrorMessage = "Service name can contian 25 characters maximum")]
    public string Name { get; set; }
    [Required]
    [MaxLength(100, ErrorMessage = "Service description can contain 100 characters maximum")]
    public string Description { get; set; }
    [Required]
    public string Icon { get; set; }
}

}

