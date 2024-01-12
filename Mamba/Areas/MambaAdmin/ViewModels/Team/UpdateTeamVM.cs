using Mamba.Models;
using System.ComponentModel.DataAnnotations;

namespace Mamba.Areas.MambaAdmin.ViewModels.Team
{
    public class UpdateTeamVM
    {
        
        [MaxLength(25, ErrorMessage = "Name can contain maximum 25 characters")]
        public string Name { get; set; }

        
        [MaxLength(25, ErrorMessage = "Surname can contain maximum 25 characters")]
        public string Surname { get; set; }
        
        public IFormFile? Photo { get; set; }
        public string? ImageUrl { get; set; }
        public int? PositionId { get; set; }
        public List<Position>? Positions { get; set; }
    }
}
