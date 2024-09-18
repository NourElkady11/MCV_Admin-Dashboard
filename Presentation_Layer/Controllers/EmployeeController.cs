using AutoMapper;
using BusniussLogic_Layer.Repositories;
using DataAccess_Layer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Presentation_Layer.Utilities;
using Presentation_Layer.ViewModels;

namespace Presentation_Layer.Controllers
{
    public class EmployeeController : Controller
    {
   
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public EmployeeController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.mapper= mapper;
        }
        public IActionResult Index(string searchValue)
        {
            if (string.IsNullOrWhiteSpace(searchValue))
            {

                  var Employees = unitOfWork.Employees.GetAllWithDepartment();
                  var EmployeeVM = mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(Employees);
                  return View(EmployeeVM);
            }
            else
            {
                var employees = unitOfWork.Employees.GetAllEmployees(searchValue);
                var EmployeeVM = mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
                return View(EmployeeVM);

            }
        }


        public IActionResult Create()
        {

            var departments= unitOfWork.Departments.GetAll();
            SelectList listItems = new SelectList(departments,"Id","Name");
            ViewBag.departments=listItems;
            //this (Id)=>DataValue filed we will send it to the view
            return View();
        }


        [HttpPost]
        public IActionResult Create(EmployeeViewModel EmployeeViewModel)
        {
            if (EmployeeViewModel.Image is not null)
            {
                EmployeeViewModel.ImageName=DocumentSetting.uploadFile(EmployeeViewModel.Image, "Images");
            }
            var employee = mapper.Map<EmployeeViewModel, Employee>(EmployeeViewModel);


            if (!ModelState.IsValid)
            {
                return View(EmployeeViewModel);
            }
            else
            {
               
                unitOfWork.Employees.Create(employee);
                unitOfWork.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
        }




        public IActionResult Details(int? id) => EditandDelete(id, nameof(Details));



        public IActionResult Edit(int? id) => EditandDelete(id, nameof(Edit));
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit([FromForm] int id, EmployeeViewModel EmployeeViewModel)
        {
            if (id != EmployeeViewModel.Id)
            {
                return BadRequest();
            }

            if (EmployeeViewModel.Image is not null)
            {
                EmployeeViewModel.ImageName = DocumentSetting.uploadFile(EmployeeViewModel.Image, "Images");
            }
      
            if (ModelState.IsValid)
            {
                try
                {
                    var employee = mapper.Map<EmployeeViewModel, Employee>(EmployeeViewModel);
                    unitOfWork.Employees.Update(employee);
                    if (unitOfWork.SaveChanges()>0)
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

            return View(EmployeeViewModel);
        }


        public IActionResult Delete(int? id) => EditandDelete(id, nameof(Delete));

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Delete(EmployeeViewModel Employee_View_Model)
        {
            if (Employee_View_Model.ImageName is not null)
            {
                DocumentSetting.DeleteFile("Images", Employee_View_Model.ImageName);
            }
            try
            {
                var employee = mapper.Map<EmployeeViewModel, Employee>(Employee_View_Model);
                unitOfWork.Employees.Delete(employee);
                if (unitOfWork.SaveChanges() > 0)
                {
                    TempData["Message2"] = "Employee Deleted";
                  
                }
               
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View(Employee_View_Model);


        }



        private IActionResult EditandDelete(int? id, string viewname)
        {
            if (viewname == nameof(Edit))
            {
                var departments = unitOfWork.Departments.GetAll();
                SelectList listItems = new SelectList(departments, "Id", "Name");
                //this (Id)=>DataValue filed we will send it to the view and set as departmentId
                ViewBag.departmetns = listItems;
            }
            if (!id.HasValue) return BadRequest();
            var employee = unitOfWork.Employees.Get(id.Value);
            if (employee is null) return NotFound();
            var EmployeeVM  = mapper.Map<Employee,EmployeeViewModel>(employee);
            return View(viewname, EmployeeVM);
        }

    }
}
