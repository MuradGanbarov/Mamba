using System.ComponentModel.DataAnnotations;

namespace Mamba.Areas.MambaAdmin.ViewModels
{
    public class UpdateServiceVM
{
    [MaxLength(25, ErrorMessage = "Name can contain maximum 25 characters")]
    public string? Name { get; set; }
    [MaxLength(100, ErrorMessage = "Description can contain maximum 100 characters")]
    public string? Description { get; set; }
    public string? Icon { get; set; }
}

}

