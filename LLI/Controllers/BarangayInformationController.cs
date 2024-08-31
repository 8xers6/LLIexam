using LLI.Data;
using LLI.Models;
using LLI.Models.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Numerics;
using Microsoft.AspNetCore.Authorization;
using ClosedXML.Excel;

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


        [HttpPost]
        public async Task<IActionResult> Delete(BarangayInformation viewModel)
        {

            var resident = await dbContext.BrgyInformation.AsNoTracking().FirstOrDefaultAsync(x=>x.Id==viewModel.Id);
            if (resident is not null) {
                dbContext.BrgyInformation.Remove(viewModel);
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("List", "BarangayInformation");
        }



        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken] // Optional: Protect against CSRF attacks
        public async Task<IActionResult> Logout()
        {
            // Sign the user out of the authentication scheme
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect to the login page after logout
            return RedirectToAction("Login", "Account");
        }



        [HttpPost]
        public ActionResult ExportToExcel()
        {
            // Fetch actual data from the database
            var residents = dbContext.BrgyInformation.ToList(); // Replace 'Residents' with your actual DbSet property name

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Residents");

                // Add header row
                worksheet.Cell(1, 1).Value = "Id";
                worksheet.Cell(1, 2).Value = "Name";
                worksheet.Cell(1, 3).Value = "Birthday";
                worksheet.Cell(1, 4).Value = "Address";
                worksheet.Cell(1, 5).Value = "Phone";
                worksheet.Cell(1, 6).Value = "EmergencyContactName";
                worksheet.Cell(1, 7).Value = "EmergencyContact";
                worksheet.Cell(1, 8).Value = "Email";

                // Add data rows
                for (int i = 0; i < residents.Count; i++)
                {
                    var resident = residents[i];
                    worksheet.Cell(i + 2, 1).Value = resident.Id.ToString(); // Convert Guid to string
                    worksheet.Cell(i + 2, 2).Value = resident.Name;
                    worksheet.Cell(i + 2, 3).Value = resident.Birthday;
                    worksheet.Cell(i + 2, 4).Value = resident.Address;
                    worksheet.Cell(i + 2, 5).Value = resident.Phone;
                    worksheet.Cell(i + 2, 6).Value = resident.EmergencyContactName;
                    worksheet.Cell(i + 2, 7).Value = resident.EmergencyContact;
                    worksheet.Cell(i + 2, 8).Value = resident.Email;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var fileName = "ResidentsReport.xlsx";
                    var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    return File(stream.ToArray(), contentType, fileName);
                }
            }
        }
    }

  }

        






