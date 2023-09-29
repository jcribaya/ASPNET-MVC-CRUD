using ASPNETMVCCRUD.Data;
using ASPNETMVCCRUD.Models;
using ASPNETMVCCRUD.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPNETMVCCRUD.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;

        public EmployeesController(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpGet]
        public  async Task<IActionResult> Index() 
        {
            var employees = await applicationDbContext.Employees.ToListAsync();
            return View(employees);

        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeModel addEmployeeModel)
        {
            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeModel.Name,
                Email = addEmployeeModel.Email,
                Salary = addEmployeeModel.Salary,
                DateofBirth = addEmployeeModel.DateofBirth,
                Department = addEmployeeModel.Department
            };

            await applicationDbContext.Employees.AddAsync(employee);
            await applicationDbContext.SaveChangesAsync();
            return RedirectToAction("Index");   
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var employee = await applicationDbContext.Employees.FirstOrDefaultAsync(find => find.Id == id);

            if (employee != null)
            {
                var employeeViewModel = new UpdateEmployeeModel()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    DateofBirth = employee.DateofBirth,
                    Department = employee.Department
                };

                return await Task.Run(() => View("View", employeeViewModel));
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateEmployeeModel model)
        {
            var employee = await applicationDbContext.Employees.FindAsync(model.Id);

            if(employee != null)
            {
                employee.Name = model.Name;
                employee.Email = model.Email;   
                employee.Salary = model.Salary;
                employee.DateofBirth = model.DateofBirth;
                employee.Department = model.Department;

                await applicationDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateEmployeeModel model)
        {
            var employee = await applicationDbContext.Employees.FindAsync(model.Id);

            if (employee != null)
            {
                applicationDbContext.Employees.Remove(employee);
                await applicationDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}
