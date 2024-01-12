using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;

namespace Mamba.Models
{
    public class Portfolio
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
