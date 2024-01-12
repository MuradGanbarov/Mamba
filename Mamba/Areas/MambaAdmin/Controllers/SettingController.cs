using Mamba.Areas.MambaAdmin.ViewModels;
using Mamba.Areas.MambaAdmin.ViewModels.Settings;
using Mamba.DAL;
using Mamba.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mamba.Areas.MambaAdmin.Controllers
{
    [Area("MambaAdmin")]
    public class SettingController : Controller
    {
        private readonly AppDbContext _context;

        public SettingController(AppDbContext context)
        {
           _context = context;
        }
        public async Task<IActionResult> Index(int page)
        {
            double count = await _context.Settings.CountAsync();
            List<Setting> settings = await _context.Settings.Skip(page * 3).Take(3).ToListAsync();

            PaginationVM<Setting> vm = new()
            {
                CurrentPage = page+1,
                TotalPage = Math.Ceiling(count/3),
                Items = settings
            };
            
            return View(vm);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(CreateSettingVM vm)
        {
            if(!ModelState.IsValid) return View(vm);

            bool check = await _context.Settings.AnyAsync(s => s.Key.ToLower().Trim() == vm.Key.ToLower().Trim()&&s.Value.ToLower().Trim()==vm.Value.ToLower().Trim());
            if(check)
            {
                ModelState.AddModelError("Key", "This setting already exists");
                return View(vm);
            }

            Setting setting = new()
            {
                Key = vm.Key,
                Value = vm.Value,
            };

            await _context.Settings.AddAsync(setting);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
            
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();

            Setting existed = await _context.Settings.FirstOrDefaultAsync(s=>s.Id == id);
            if (existed is null) return NotFound();

            UpdateSettingVM vm = new()
            {
                Key = existed.Key,
                Value = existed.Value,
            };
            return View(vm);
           
        }

        [HttpPost]

        public async Task<IActionResult> Update(int id,UpdateSettingVM vm)
        {
            if(id<=0) return BadRequest();
            Setting existed = await _context.Settings.FirstOrDefaultAsync(s => s.Id == id);
            if (existed is null) return NotFound();
            bool check = await _context.Settings.AnyAsync(s => s.Key.ToLower().Trim() == vm.Key.ToLower().Trim() && s.Value.ToLower().Trim() == vm.Value.ToLower().Trim());
            if(check)
            {
                ModelState.AddModelError("Key", "This setting already exists");
                return View(vm);
            }

            existed.Key = vm.Key;
            existed.Value = vm.Value;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Delete(int id)
        {
            if(id<0) return BadRequest();
            Setting existed = await _context.Settings.FirstOrDefaultAsync(s => s.Id == id);
            if (existed is null) return NotFound();
            _context.Settings.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }




    }
}
