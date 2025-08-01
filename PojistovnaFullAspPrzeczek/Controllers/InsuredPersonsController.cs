using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PojistovnaFullAspPrzeczek.DTOs.InsuredPersons;
using PojistovnaFullAspPrzeczek.Interfaces;

namespace PojistovnaFullAspPrzeczek.Controllers
{
    public class InsuredPersonsController : Controller
    {
        private readonly IInsuredPersonService _insuredPersonService; 
        private readonly IPersonInsuranceService _personInsuranceService; 

        public InsuredPersonsController(IInsuredPersonService insuredPersonService, IPersonInsuranceService personInsuranceService)
        {
            _insuredPersonService = insuredPersonService;
            _personInsuranceService = personInsuranceService;
        }

        [Authorize(Roles = "Admin,InsuredPerson")]
        public async Task<IActionResult> Index()
        {
            var insuredPeople = await _insuredPersonService.GetAllAsync(); 
            return View(insuredPeople);
        }

        [Authorize(Roles = "Admin,InsuredPerson")]
        // GET: InsuredPersons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var insuredPerson = await _insuredPersonService.GetByIdAsync(id.Value);

            if (insuredPerson == null) return NotFound();

            return View(insuredPerson);
        }

        /// <summary>
        /// InsuredPerson create implemnted in registration use case, separate implementation here idled for future use – admin fallback create
        /// </summary>
        /// <returns></returns>

        // GET: InsuredPersons/Edit/5
        [Authorize(Roles = "Admin,InsuredPerson")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var insuredPerson = await _insuredPersonService.GetEditByIdAsync(id.Value); 

            if (insuredPerson == null) return NotFound();

            return View(insuredPerson);
        }

        // POST: InsuredPersons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,InsuredPerson")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, InsuredPersonEditDto insuredPerson)
        {
            if (id != insuredPerson.IdInsuredPerson)    return NotFound();
            
            if (!ModelState.IsValid) return View(insuredPerson);

            try
            {
                await _insuredPersonService.UpdateAsync(insuredPerson); 
                TempData["ToastMessage"] = "Údaje pojištěnce byly úspěšně upraveny.";
                TempData["ToastType"] = "success";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(nameof(insuredPerson.Email), ex.Message);
                return View(insuredPerson);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Nastala neočekávaná chyba: " + ex.Message);
                return View(insuredPerson);
            }
        }

        // GET: InsuredPersons/Delete/5
        [Authorize(Roles = "Admin,InsuredPerson")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var insuredPerson = await _insuredPersonService.GetDeleteByIdAsync(id.Value); 
                
            if (insuredPerson == null) return NotFound();

            return View(insuredPerson);
        }

        // POST: InsuredPersons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,InsuredPerson")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _insuredPersonService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        // GET: InsuredPerson/ChangeInsurance/5
        [Authorize(Roles = "Admin,InsuredPerson")]
        public async Task<IActionResult> ChangeInsurance(int id)
        {
            var dto = await _personInsuranceService.GetSelectableForInsuredPerson(id); 
            if (dto == null) return NotFound();

            return View(dto);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeInsurance(int id, int selectedInsuranceId)
        {
            try
            {
                var result = await _personInsuranceService.AssignInsuranceToInsuredPerson(id, selectedInsuranceId);
                if (!result) return NotFound();

                return RedirectToAction(nameof(Details), new { id });
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                var dto = await _personInsuranceService.GetSelectableForInsuredPerson(id);
                return View(dto);
            }
        }
    }
}
