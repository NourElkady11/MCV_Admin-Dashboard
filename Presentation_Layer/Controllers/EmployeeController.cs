using BusniussLogic_Layer.Repositories;
using DataAccess_Layer.Models;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.Controllers
{
    public class EmployeeController : Controller
    {
        private IEmployeeRepoistory employeeRepoistory;

        public EmployeeController(IEmployeeRepoistory employeeRepoistory)
        {
            this.employeeRepoistory = employeeRepoistory;
        }
        public IActionResult Index()
        {
            /*            ViewData["message"] = "Hello ya zmiksy";*/
            ViewBag.Message = "Hello from Bag";
            var Employees = employeeRepoistory.GetAll();
            return View(Employees);
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Employee Employee)
        {
            if (!ModelState.IsValid)
            {
                return View(Employee);
            }
            else
            {
                employeeRepoistory.Create(Employee);
                return RedirectToAction(nameof(Index));
            }
        }




        public IActionResult Details(int? id) => EditandDelete(id, nameof(Details));



        public IActionResult Edit(int? id) => EditandDelete(id, nameof(Edit));
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit([FromForm] int id, Employee Employee)
        {
            if (id != Employee.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (employeeRepoistory.Update(Employee) > 0)
                    {
                        TempData["Message"] = "Employee updated";
                    }
                   
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }

            }

            return View(Employee);
        }


        public IActionResult Delete(int? id) => EditandDelete(id, nameof(Delete));

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Delete(Employee Employee)
        {

            try
            {
                if (employeeRepoistory.Delete(Employee) > 0)
                {
                    TempData["Message2"] = "Employee Deleted";
                }
               
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View(Employee);


        }



        private IActionResult EditandDelete(int? id, string viewname)
        {
            if (!id.HasValue) return BadRequest();
            var dept = employeeRepoistory.Get(id.Value);
            if (dept is null) return NotFound();
            return View(viewname, dept);
        }

    }
}
