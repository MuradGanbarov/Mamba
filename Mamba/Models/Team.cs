namespace Mamba.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? ImageUrl { get; set; }
        public int? PositionId { get; set; }
        public Position? Position { get; set; }
    }
}
