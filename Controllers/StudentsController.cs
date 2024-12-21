using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.web.Data;
using StudentPortal.web.Models;
using StudentPortal.web.Models.Entities;

namespace StudentPortal.web.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        public StudentsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public  async Task <IActionResult> Add(AddStudentViewModel viewModel)
        {
            var student = new Student
            {
                Name =viewModel.Name,   
                Email =viewModel.Email,   
                Phone =viewModel.Phone,   
                Subscribed =viewModel.Subscribed   
            };
            await dbContext.Students.AddAsync(student);
            await dbContext.SaveChangesAsync();
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var students = await dbContext.Students.ToListAsync();
            return View(students);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var students = await dbContext.Students.FindAsync(id);
            return View(students);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Student viewmodel)
        {
            var students = await dbContext.Students.FindAsync(viewmodel.Id);
            if (students is not null)
            {
                students.Name = viewmodel.Name;
                students.Email = viewmodel.Email;   
                students.Phone = viewmodel.Phone;
                students.Subscribed = viewmodel.Subscribed;

                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("List", "Students");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Student viewmodel)
        {
            var student = await dbContext.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == viewmodel.Id);
            if (student is not null)
            {
                dbContext.Students.Remove(viewmodel);
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("List", "Students");
        }
    }
}
