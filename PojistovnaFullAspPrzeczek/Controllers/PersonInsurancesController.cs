using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PojistovnaFullAspPrzeczek.DTOs.PersonInsurances;
using PojistovnaFullAspPrzeczek.Interfaces;
using PojistovnaFullAspPrzeczek.Models;

namespace PojistovnaFullAspPrzeczek.Controllers
{
    public class PersonInsurancesController(IPersonInsuranceService personInsuranceService, IInsuranceService insuranceService) : Controller
    {
        // GET: PersonInsurances
        public async Task<IActionResult> Index()
        {
            return View(await personInsuranceService.GetAllAsync());
        }

        // GET: PersonInsurances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personInsurance = await personInsuranceService.GetByIdAsync(id.Value);
            if (personInsurance == null)
            {
                return NotFound();
            }

            return View(personInsurance);
        }

        // GET: PersonInsurances/Create
        public async Task<IActionResult> Create(int idInsuredPerson)
        {
            var insurances = await insuranceService.GetAllAsync();
            ViewBag.InsuranceList = new SelectList(insurances, "IdInsurance", "Title");

            var model = new PersonInsuranceCreateDto
            {
                IdInsuredPerson = idInsuredPerson,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddYears(1) // volitelné defaulty
            };

            return View(model); // ✅ model bude null-proof
        }

        // POST: PersonInsurances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PersonInsuranceCreateDto dto)
        {
            if (ModelState.IsValid)
            {
                await personInsuranceService.CreateAsync(dto);
                return RedirectToAction("Details", "InsuredPersons", new { id = dto.IdInsuredPerson });
            }

            var insurances = await insuranceService.GetAllAsync();
            ViewBag.InsuranceList = new SelectList(insurances, "IdInsurance", "Title", dto.IdInsuranceForPerson);

            return View(dto);
        }

        // GET: PersonInsurances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var personInsurance = await personInsuranceService.GetEditByIdAsync(id.Value);
            if (personInsurance == null) return NotFound();

            var insurances = await insuranceService.GetAllAsync();
            ViewBag.InsuranceList = new SelectList(insurances, "IdInsurance", "Title", personInsurance.IdInsuranceForPerson);

            return View(personInsurance);
        }

        // POST: PersonInsurances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPersonInsurance,InsurancePrice,SubjectOfInsurance,IdInsuranceForPerson,IdInsuredPerson,StartDate,EndDate")] PersonInsuranceDto personInsurance)
        {
            if (id != personInsurance.IdPersonInsurance) return NotFound();

            if (ModelState.IsValid)
            {
                await personInsuranceService.UpdateAsync(personInsurance);
                
                TempData["ToastMessage"] = "Detaily pojištění byly úspěšně upraveny.";
                TempData["ToastType"] = "success";

                return RedirectToAction("Details", "InsuredPersons", new { id = personInsurance.IdInsuredPerson });
            }

            var insurances = await insuranceService.GetAllAsync();
            ViewBag.InsuranceList = new SelectList(insurances, "IdInsurance", "Title", personInsurance.IdInsuranceForPerson);

            return View(personInsurance);
        }

        // GET: PersonInsurances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personInsurance = await personInsuranceService.GetDeleteByIdAsync(id.Value);
            if (personInsurance == null)
            {
                return NotFound();
            }

            return View(personInsurance);
        }

        // POST: PersonInsurances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int IdInsuredPerson)
        {
            var personInsurance = await personInsuranceService.GetByIdAsync(id); // _context.PersonInsurance.FindAsync(id);
            if (personInsurance != null)
            {
                await personInsuranceService.DeleteAsync(personInsurance.IdPersonInsurance); // _context.PersonInsurance.Remove(personInsurance);
            }

            return RedirectToAction("Details", "InsuredPersons", new { id = IdInsuredPerson });
        }
    }
}
