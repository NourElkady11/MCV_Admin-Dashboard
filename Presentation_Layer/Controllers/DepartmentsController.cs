using BusniussLogic_Layer.Repositories;
using DataAccess_Layer.Data;
using DataAccess_Layer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Presentation_Layer.Controllers
{
    public class DepartmentsController : Controller
    {
        private IDepartmentRepositorys departmentRepo;

        public DepartmentsController(IDepartmentRepositorys departmentRepo)
        {
            this.departmentRepo = departmentRepo;
        }
        public IActionResult Index()
        {
            var departments = departmentRepo.GetAll();
            return View(departments);
        }


        public IActionResult Create() { 
            return View();
        }


        [HttpPost]
        public IActionResult Create(Department department)
        {
            if (!ModelState.IsValid)
            { 
                return View(department);
            }
            else
            {
                departmentRepo.Create(department);
                return RedirectToAction(nameof(Index));
            }       
        }


       
       
        public IActionResult Details(int? id)=> EditandDelete(id, nameof(Details));



        public IActionResult Edit(int? id) => EditandDelete(id, nameof(Edit));
        [HttpPost,ValidateAntiForgeryToken]
        public IActionResult Edit([FromForm]int id,Department department)
        {
            if (id != department.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    departmentRepo.Update(department);
                   return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }

            }
       
            return View(department);
        }


        public IActionResult Delete(int? id)=>EditandDelete(id, nameof(Delete));

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Delete(Department department)
        {
           
            try
            {
                departmentRepo.Delete(department);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View(department);


        }



        private IActionResult EditandDelete(int? id, string viewname)
        {
            if (!id.HasValue) return BadRequest();
            var dept = departmentRepo.Get(id.Value);
            if (dept is null) return NotFound();
            return View(viewname, dept);
        }

    }
}
