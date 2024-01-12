using System.ComponentModel.DataAnnotations;

namespace Mamba.Areas.MambaAdmin.ViewModels
{
    public class CreatePositionVM
    {
        [Required]
        [MaxLength(25, ErrorMessage = "Position name can contain maximum 25 characters")]
        public string Name { get; set; }
    }
}
