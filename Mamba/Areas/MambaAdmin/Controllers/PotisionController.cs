using Mamba.Areas.MambaAdmin.ViewModels;
using Mamba.DAL;
using Mamba.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mamba.Areas.MambaAdmin.Controllers
{
    [Area("MambaAdmin")]
    public class PotisionController : Controller
    {
        private readonly AppDbContext _context;

        public PotisionController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int page)
        {
            double count = await _context.Positions.CountAsync();
            List<Position> positions = await _context.Positions.Include(p => p.Teams).Skip(page * 3).Take(3).ToListAsync();
            PaginationVM<Position> vm = new()
            {
                CurrentPage = page,
                TotalPage = Math.Ceiling(count/3),
                Items = positions
            };

            return View(vm);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(CreatePositionVM vm)
        {
            if (!ModelState.IsValid) return View(vm);
            bool check = await _context.Positions.Include(p=>p.Teams).AnyAsync(p=>p.Name.ToLower().Trim()==vm.Name.ToLower().Trim());
            if(check)
            {
                ModelState.AddModelError("Name", "This position already exists");
                return View(vm);
            }

            Position position = new()
            {
                Name = vm.Name,
            };
            
            await _context.Positions.AddAsync(position);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Position position = await _context.Positions.Include(p=>p.Teams).FirstOrDefaultAsync(p=>p.Id==id);
            if(position == null) return BadRequest();
            UpdatePositionVM vm = new()
            {
                Name = position.Name,
            };

            return View(vm);

        }

        [HttpPost]
        
        public async Task<IActionResult> Update(int id,UpdatePositionVM vm)
        {
            if(id <= 0) return BadRequest();
            Position existed = await _context.Positions.Include(p => p.Teams).FirstOrDefaultAsync(p => p.Id == id);
            if (existed == null) return BadRequest();
            bool check = await _context.Positions.Include(p => p.Teams).AnyAsync(p => p.Name.ToLower().Trim() == vm.Name.ToLower().Trim());
            if (check)
            {
                ModelState.AddModelError("Name", "This position already exists");
                return View(vm);
            }

            existed.Name = vm.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Position position = await _context.Positions.Include(p => p.Teams).FirstOrDefaultAsync(p => p.Id == id);
            if(position == null) return BadRequest();
            _context.Remove(position);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }




    }
}
