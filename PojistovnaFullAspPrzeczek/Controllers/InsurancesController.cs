using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PojistovnaFullAspPrzeczek.DTOs.Insurances;
using PojistovnaFullAspPrzeczek.Interfaces;

namespace PojistovnaFullAspPrzeczek.Controllers
{
    public class InsurancesController : Controller
    {
        private readonly IInsuranceService _insuranceService;

        public InsurancesController(IInsuranceService insuranceService)
        {
            _insuranceService = insuranceService;
        }

        // GET: Insurances
        public async Task<IActionResult> Index()
        {
            return View(await _insuranceService.GetAllAsync());
        }

        // GET: Insurances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insurance = await _insuranceService.GetByIdAsync(id.Value);
                
            if (insurance == null)
            {
                return NotFound();
            }

            return View(insurance);
        }

        [Authorize(Roles = "Admin")]
        // GET: Insurances/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Insurances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdInsurance,Title")] InsuranceDto insuranceDto)
        {
            if (ModelState.IsValid)
            {
                await _insuranceService.CreateAsync(insuranceDto);
            }
            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = "Admin")]
        // GET: Insurances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var insurance = await _insuranceService.GetEditByIdAsync(id.Value);

            if (insurance == null) return NotFound();

            return View(insurance);
        }

        // POST: Insurances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdInsurance,Title")] InsuranceDto insuranceDto)
        {
            if (id != insuranceDto.IdInsurance)    return NotFound();

            if (!ModelState.IsValid) return View(insuranceDto);

            try
            {
                await _insuranceService.UpdateAsync(insuranceDto);
                TempData["ToastMessage"] = "Detaily pojištění byly úspěšně upraveny.";
                TempData["ToastType"] = "success";
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                // fallback – můžeš přizpůsobit nebo logovat
                ModelState.AddModelError(string.Empty, "Nastala neočekávaná chyba: " + ex.Message);
                return View(insuranceDto);
            }
        }

        // GET: Insurances/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var insuranceDto = await _insuranceService.GetDeleteByIdAsync(id.Value);

            if (insuranceDto == null) return NotFound();

            return View(insuranceDto);
        }

        // POST: Insurances/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _insuranceService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }
    }
}
