using System.ComponentModel.DataAnnotations;

namespace Mamba.Areas.MambaAdmin.ViewModels
{
    public class UpdateCategoryVM
    {
        [MaxLength(25, ErrorMessage = "Category name can contain maximum 25 characters")]
        public string Name { get; set; }
    }
}
