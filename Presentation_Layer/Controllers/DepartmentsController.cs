using AutoMapper;
using BusniussLogic_Layer.Repositories;
using DataAccess_Layer.Data;
using DataAccess_Layer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Presentation_Layer.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public DepartmentsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var departments = await unitOfWork.Departments.GetAllAsync();
            return View(departments);
        }


        public IActionResult Create() { 
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Department department)
        {
            if (!ModelState.IsValid)
            { 
                return View(department);
            }
            else
            {
               await unitOfWork.Departments.CreateAsync(department);
               await unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }       
        }


       
       
        public async Task<IActionResult> Details(int? id)=> await EditandDelete(id, nameof(Details));



        public async Task<IActionResult> Edit(int? id) => await EditandDelete(id, nameof(Edit));
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
                    unitOfWork.Departments.Update(department);
                    unitOfWork.SaveChangesAsync();
                   return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }

            }
       
            return View(department);
        }


        public async Task<IActionResult> DeleteAsync(int? id)=>await EditandDelete(id, nameof(Delete));

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Delete(Department department)
        {
           
            try
            {
                unitOfWork.Departments.Delete(department);
                unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View(department);


        }



        private async Task<IActionResult> EditandDelete(int? id, string viewname)
        {
        

            if (!id.HasValue) return BadRequest();
            var dept = await unitOfWork.Departments.GetAsync(id.Value);
            if (dept is null) return NotFound();
            return View(viewname, dept);
        }

    }
}
