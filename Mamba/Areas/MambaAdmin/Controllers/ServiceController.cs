using Mamba.Areas.MambaAdmin.ViewModels;
using Mamba.DAL;
using Mamba.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mamba.Areas.MambaAdmin.Controllers
{
    [Area("MambaAdmin")]
    public class ServiceController : Controller
    {
        private readonly AppDbContext _context;

        public ServiceController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int page)
        {
            double count = await _context.Services.CountAsync();
            List<Service> services = await _context.Services.Skip(page* 3).Take(3).ToListAsync();

            PaginationVM<Service> vm = new()
            {
                CurrentPage = page+1,
                TotalPage = Math.Ceiling(count/3),
                Items = services,
            };

            return View(vm);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(CreateServiceVM vm)
        {
            if (!ModelState.IsValid) return View(vm);
            bool check = await _context.Services.AnyAsync(s => s.Name.ToLower().Trim() == vm.Name.ToLower().Trim());
            if (check)
            {
                ModelState.AddModelError("Name", "This service already exists");
                return View(vm);
            }

            Service newService = new()
            {
                Name = vm.Name,
                Description = vm.Description,
                Icon = vm.Icon
            };

            await _context.AddAsync(newService);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Service service = await _context.Services.FirstOrDefaultAsync(s => s.Id == id);
            if (service is null) return NotFound();
            
            UpdateServiceVM vm = new()
            {
                Name = service.Name,
                Description = service.Description,
                Icon = service.Icon,
            };
            return View(vm);
        }

        [HttpPost]

        public async Task<IActionResult> Update(int id,UpdateServiceVM vm)
        {
            if(id<=0) return BadRequest();
            if (!ModelState.IsValid) return View(vm);
            Service existed = await _context.Services.FirstOrDefaultAsync(s => s.Id == id);
            if (existed is null) return NotFound();
            bool check = await _context.Services.AnyAsync(s => s.Name.ToLower().Trim() == vm.Name.ToLower().Trim());
            if (check)
            {
                ModelState.AddModelError("Name", "This service already exists");
                return View(vm);
            }

            existed.Name = vm.Name;
            existed.Description = vm.Description;
            existed.Icon = vm.Icon;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Service service = await _context.Services.FirstOrDefaultAsync(s => s.Id == id);
            if (service is null) return NotFound();
            _context.Remove(service);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }


    }
}
