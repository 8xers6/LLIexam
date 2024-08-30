using LLI.Data;
using LLI.Models;
using LLI.Models.Entities;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Numerics;

namespace LLI.Controllers
{
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


            return View();
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
            if(resident is not null){

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








    }

        





}
