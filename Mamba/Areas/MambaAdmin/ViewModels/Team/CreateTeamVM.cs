using Mamba.Models;
using System.ComponentModel.DataAnnotations;

namespace Mamba.Areas.MambaAdmin.ViewModels.Team
{
    public class CreateTeamVM
    {
        [Required]
        [MaxLength(25,ErrorMessage ="Name can contain maximum 25 characters")]
        public string Name { get; set; }

        [Required]
        [MaxLength(25, ErrorMessage = "Surname can contain maximum 25 characters")]
        public string Surname { get; set; }
        [Required(ErrorMessage ="You need upload photo")]
        public IFormFile? Photo { get; set; }
        [Required(ErrorMessage ="You need select position")]
        public int? PositionId { get; set; }
        public List<Position>? Positions { get; set; }
    }
}
