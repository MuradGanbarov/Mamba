using Mamba.Areas.MambaAdmin.ViewModels;
using Mamba.DAL;
using Mamba.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mamba.Areas.MambaAdmin.Controllers
{
    [Area("MambaAdmin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int page)
        {
            double count = await _context.Categories.CountAsync();
            List<Category> categories = await _context.Categories.Include(c=>c.Portfolios).Skip(page * 3).Take(3).ToListAsync();

            PaginationVM<Category> vm = new()
            {
                CurrentPage = page+1,
                TotalPage = Math.Ceiling(count / 3),
                Items = categories,
            };

            return View(vm);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(CreateCategoryVM vm)
        {
            if (!ModelState.IsValid) return View();

            bool check = await _context.Categories.AnyAsync(c=>c.Name.ToLower().Trim()==vm.Name.ToLower().Trim());
            if(check)
            {
                ModelState.AddModelError("Name", "This category already exists");
                return View();
            }

            Category category = new()
            {
                Name = vm.Name,
            };

            await _context.AddAsync(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();

            Category existed = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (existed is null) return NotFound();

            UpdateCategoryVM vm = new()
            {
                Name = existed.Name
            };

            return View(vm);

        }

        [HttpPost]

        public async Task<IActionResult> Update(int id, UpdateCategoryVM vm)
        {
            if (!ModelState.IsValid) return View(vm);
            if(id<=0) return BadRequest();
            Category existed = await _context.Categories.FirstOrDefaultAsync(c=>c.Id==id);
            if(existed is null) return NotFound();
            bool check = await _context.Categories.AnyAsync(c=>c.Name.ToLower().Trim()== vm.Name.ToLower().Trim());
            if (check)
            {
                ModelState.AddModelError("Name", "This category already exists");
                return View(vm);
            }

            existed.Name= vm.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        
        public async Task<IActionResult> Delete(int id)
        {
            if(id <= 0) return BadRequest();
            Category category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if(category is null) return NotFound();

            _context.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }






    }
}
