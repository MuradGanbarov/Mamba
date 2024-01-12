using System.ComponentModel.DataAnnotations;

namespace Mamba.Areas.MambaAdmin.ViewModels
{
    public class CreateCategoryVM
    {
        [MaxLength(25, ErrorMessage = "Category name can contain 25 characters")]
        public string Name { get; set; }
    }
}
