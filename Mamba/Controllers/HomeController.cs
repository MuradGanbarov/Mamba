using Mamba.DAL;
using Mamba.Models;
using Mamba.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mamba.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
           _context = context;
        }
        public async Task<IActionResult> Index()
        {
            HomeVM vm = new()
            {
                Portfolios = await _context.Portfolios.ToListAsync(),
                Services = await _context.Services.ToListAsync(),
                Teams = await _context.Teams.ToListAsync(),
                
            };
            return View(vm);
        }
    }
}
