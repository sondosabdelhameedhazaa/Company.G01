using AutoMapper;
using Company.G01.BLL.Interfaces;
using Company.G01.BLL.Repositories;
using Company.G01.DAL.Models;
using Company.G01.PL.Dtos;
using Company.G01.PL.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.G01.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IEmployeeRepository _employeeRepository;
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        //ASK CLR  to Create Object From DepartmentRepository

        public EmployeeController(
            //  IEmployeeRepository employeeRepository
            //, IDepartmentRepository departmentRepository
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            //_employeeRepository = employeeRepository;
            //_departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        [HttpGet] // Get: /Department/Index
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
            }
            else
            {
                employees = await _unitOfWork.EmployeeRepository.GetByNameAsync(SearchInput);
            }





            // Dictionary :
            // 1. ViewData : transfer extra information from controller to view
            // ViewData["Message"] = " Hello From View Data ";

            // 2. ViewBag  : transfer extra information from controller to view
            //ViewBag.Message = " Hello From View Bag";


            // 3. TempData

            return View(employees);
        }



        [HttpGet]
        public async Task<IActionResult> Search(string? SearchInput)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                employees = await _unitOfWork.EmployeeRepository.GetAllAsync();

            }
            else
            {
                employees = await _unitOfWork.EmployeeRepository.GetByNameAsync(SearchInput);
            }

            return PartialView("EmployeePartialView/EmployeesTablePartialView", employees);
        }




        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            ViewData["departments"] = departments;
            if (ViewData["departments"] == null)
            {
                Console.WriteLine("ViewData['departments'] is NULL!");
            }
            else
            {
                Console.WriteLine("ViewData['departments'] has data!");
            }
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeDto model)
        {
            //Manual Mapping

            if (ModelState.IsValid) //Server Side Validation
            {
                try
                {
                    if (model.Image is not null)
                    {
                        model.ImageName = DocumentSettings.UploadFile(model.Image, "images");
                    }
                    var employee = _mapper.Map<Employee>(model);

                    await _unitOfWork.EmployeeRepository.AddAsync(employee);

                    var count = await _unitOfWork.CompeleteAsync();
                    if (count > 0)
                    {
                        TempData["Message"] = "Employee is Created";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError("", ex.Message);
                }


            }
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return BadRequest("Invalid Id"); //400
            var employee = await _unitOfWork.EmployeeRepository.GetAsync(id.Value);
            if (employee is null) return NotFound(new { statusCode = 404, message = $"employee with Id :{id} is not Found" });

            var dto = _mapper.Map<CreateEmployeeDto>(employee);

            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id, string viewName = "Edit")
        {
            if (id is null) return BadRequest("Invalid Id"); // 400

            var employee = await _unitOfWork.EmployeeRepository.GetAsync(id.Value);

            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            ViewData["departments"] = departments;

            if (employee is null) return NotFound(new { statusCode = 404, message = $"Department with id : {id} is not found" });

            var dto = _mapper.Map<CreateEmployeeDto>(employee);

            return View(viewName, dto);

        }


        [HttpPost]

        public async Task<IActionResult> Edit([FromRoute] int id, CreateEmployeeDto model, string viewName = "Edit")
        {

            if (ModelState.IsValid)
            {
                if (model.ImageName is not null && model.Image is not null)
                {
                    DocumentSettings.DeleteFile(model.ImageName, "Images");
                }

                if (model.Image is not null)
                {
                    model.ImageName = DocumentSettings.UploadFile(model.Image, "Images");
                }


                var employee = _mapper.Map<Employee>(model);

                employee.Id = id;

                _unitOfWork.EmployeeRepository.Update(employee);
                var count = await _unitOfWork.CompeleteAsync();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(viewName, model);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Edit(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, CreateEmployeeDto model)
        {

            if (ModelState.IsValid)
            {
                var employee = _mapper.Map<Employee>(model);
                employee.Id = id;
                _unitOfWork.EmployeeRepository.Delete(employee);
                var count = await _unitOfWork.CompeleteAsync();

                if (count > 0)
                {
                    if (model.ImageName is not null)
                    {
                        DocumentSettings.DeleteFile(model.ImageName, "Images");
                    }
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }



    }
}