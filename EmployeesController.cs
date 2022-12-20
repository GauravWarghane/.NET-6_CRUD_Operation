using ASPNETMVCCRUD.Data;

using ASPNETMVCCRUD.Models;
using ASPNETMVCCRUD.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using ASPNETMVCCRUD.Controllers;
using Microsoft.EntityFrameworkCore;

namespace ASPNETMVCCRUD.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly MVCDemoDbContext mvcDemoDbContext;

        public EmployeesController(MVCDemoDbContext mvcDemoDbContext)
        {


            this.mvcDemoDbContext = mvcDemoDbContext;
        }

        [HttpGet]  
        public async Task<IActionResult> Index()
        {
            var employees = await mvcDemoDbContext.Empoyees.ToListAsync();
            return View(employees);
        } 

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest)
        {
            var empoyee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeRequest.Name,
                Email = addEmployeeRequest.Email,
                Salary = addEmployeeRequest.Salary,
                Department = addEmployeeRequest.Department,
                DateOfBirth = addEmployeeRequest.DateOfBirth
            };

            await mvcDemoDbContext.Empoyees.AddAsync(empoyee);
            await mvcDemoDbContext.SaveChangesAsync();

            return RedirectToAction("Add");        

        }

        [HttpGet]

        public async Task<IActionResult> View (Guid id)
        {

            var employee = await mvcDemoDbContext.Empoyees.FirstOrDefaultAsync(x => x.Id == id);
           
            if(employee!=null)
            {
                var viewModel = new UpdateEmployeeViewModel()

                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    Department = employee.Department,
                    DateOfBirth = employee.DateOfBirth
                };

                //https://www.youtube.com/watch?v=2Cp8Ti_f9Gk&t=1s video link
                return await Task.Run(()=> View("View",viewModel));
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult>View (UpdateEmployeeViewModel model)
        {
            var employee = await mvcDemoDbContext.Empoyees.FindAsync(model.Id);//

            if (employee != null)
            {
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Salary = model.Salary;
                employee.DateOfBirth = model.DateOfBirth;
                employee.Department = model.Department;

               await  mvcDemoDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult>Delete(UpdateEmployeeViewModel model)
        {
            var employee = await mvcDemoDbContext.Empoyees.FindAsync(model.Id);
            if(employee !=  null )
            {
                mvcDemoDbContext.Empoyees.Remove(employee);
                await mvcDemoDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");


        }



    }
}
