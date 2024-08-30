using LLI.Data;
using LLI.Models;
using LLI.Models.Entities;
using Microsoft.AspNetCore.Mvc;
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
    }
}
