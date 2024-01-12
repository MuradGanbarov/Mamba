using Mamba.Areas.MambaAdmin.Models.Utilities.Enums;
using Mamba.Areas.MambaAdmin.Models.Utilities.Extentions;
using Mamba.Areas.MambaAdmin.ViewModels;
using Mamba.Areas.MambaAdmin.ViewModels.Team;
using Mamba.DAL;
using Mamba.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mamba.Areas.MambaAdmin.Controllers
{
    [Area("MambaAdmin")]
    public class TeamController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public TeamController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index(int page)
        {
            double count = await _context.Teams.Include(t => t.Position).CountAsync();
            List<Team> teams = await _context.Teams.Include(t=>t.Position).Skip(page*3).Take(3).ToListAsync();
            PaginationVM<Team> vm = new()
            {
                CurrentPage = page + 1,
                TotalPage = Math.Ceiling(count / 3),
                Items = teams
            };

            return View(vm);
        }

        public async Task<IActionResult> Create()
        {
            CreateTeamVM vm = new()
            {
                Positions = await _context.Positions.ToListAsync()
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTeamVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            bool check = await _context.Positions.AnyAsync(p => p.Id == vm.PositionId);
            if (!check)
            {
                vm.Positions = await _context.Positions.ToListAsync();
                ModelState.AddModelError("PositionId", "This position doesn't exists");
            }

            if (!vm.Photo.IsValidType(FileType.Image))
            {
                vm.Positions = await _context.Positions.ToListAsync();
                ModelState.AddModelError("Image", "Photo should be image type");
                return View(vm);
            }

            if (!vm.Photo.IsValidSize(5, FileSize.Megabyte))
            {
                vm.Positions = await _context.Positions.ToListAsync();
                ModelState.AddModelError("Image", "Photo can be less or equal 5 mb");
                return View(vm);
            }

            Team team = new()
            {
                Name = vm.Name,
                Surname = vm.Surname,
                ImageUrl = await vm.Photo.CreateAsync(_env.WebRootPath,"assets","img","team"),
                PositionId = vm.PositionId,
            };
            await _context.AddAsync(team);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Team existed = await _context.Teams.Include(t=>t.Position).FirstOrDefaultAsync(t=>t.Id==id);
            if (existed is null) return NotFound();
            UpdateTeamVM vm = new()
            {
                Positions = await _context.Positions.ToListAsync()
            };
            return View(vm);

        }

        [HttpPost]

        public async Task<IActionResult> Update(int id, UpdateTeamVM vm)
        {
            if(id<=0) return BadRequest();
            Team existed = await _context.Teams.Include(t => t.Position).FirstOrDefaultAsync(t => t.Id == id);
            if (!ModelState.IsValid)
            {
                vm.Positions = await _context.Positions.ToListAsync();
                ModelState.AddModelError("Position", "This position not found");
                return View(vm);
            }
            
            if (existed is null) return NotFound();

            bool checkPosition = await _context.Positions.AnyAsync(p => p.Id == vm.PositionId);
            if (!checkPosition)
            {
                vm.Positions = await _context.Positions.ToListAsync();
                return View(vm);
            }

            if(vm.Photo is not null)
            {
                if (!vm.Photo.IsValidType(FileType.Image))
                {
                    vm.Positions = await _context.Positions.ToListAsync();
                    ModelState.AddModelError("Image", "Photo should be image type");
                    return View(vm);
                }

                if (!vm.Photo.IsValidSize(5, FileSize.Megabyte))
                {
                    vm.Positions = await _context.Positions.ToListAsync();
                    ModelState.AddModelError("Image", "Photo size can be less or equal than 5mb");
                    return View(vm);
                }

                existed.ImageUrl.DeleteAsync(_env.WebRootPath, "assets", "img", "team");
                existed.ImageUrl = await vm.Photo.CreateAsync("assets", "img", "team");

            }

            existed.Name = vm.Name;
            existed.Surname = vm.Surname;
            existed.PositionId = vm.PositionId;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Team team = await _context.Teams.Include(p => p.Position).FirstOrDefaultAsync(p => p.Id == id);
            if (team is null) return NotFound();

            team.ImageUrl.DeleteAsync(_env.WebRootPath, "assets", "img", "team");
            _context.Remove(team);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));




        }

    }
}
