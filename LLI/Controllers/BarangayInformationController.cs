using LLI.Data;
using LLI.Models;
using LLI.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LLI.Controllers
{
    [Authorize]
    public class BarangayInformationController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public BarangayInformationController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddResidentViewModel viewModel)
        {
            var resident = new BarangayInformation
            {
                Name = viewModel.Name,
                Birthday = viewModel.Birthday,
                Address = viewModel.Address,
                Phone = viewModel.Phone,
                EmergencyContactName = viewModel.EmergencyContactName,
                EmergencyContact = viewModel.EmergencyContact,
                Email = viewModel.Email
            };

            await dbContext.BrgyInformation.AddAsync(resident);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("List", "BarangayInformation");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var resident = await dbContext.BrgyInformation.ToListAsync();
            return View(resident);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var resident = await dbContext.BrgyInformation.FindAsync(id);
            return View(resident);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BarangayInformation viewModel)
        {
            var resident = await dbContext.BrgyInformation.FindAsync(viewModel.Id);
            if (resident is not null)
            {
                resident.Name = viewModel.Name;
                resident.Birthday = viewModel.Birthday;
                resident.Address = viewModel.Address;
                resident.Phone = viewModel.Phone;
                resident.EmergencyContactName = viewModel.EmergencyContactName;
                resident.EmergencyContact = viewModel.EmergencyContact;
                resident.Email = viewModel.Email;

                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("List", "BarangayInformation");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(BarangayInformation viewModel)
        {
            var resident = await dbContext.BrgyInformation.AsNoTracking().FirstOrDefaultAsync(x => x.Id == viewModel.Id);
            if (resident is not null)
            {
                dbContext.BrgyInformation.Remove(viewModel);
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("List", "BarangayInformation");
        }
    }
}
