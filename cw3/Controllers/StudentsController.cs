using System;
using cw3.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace cw3.Controllers
{
    [ApiController]
    [Route("api/students")]
    [Authorize(Roles = "student")]
    public class StudentsController : ControllerBase
    {
        private readonly UniversityContext dbContext;

        public StudentsController(UniversityContext uc)
        {
            dbContext = uc;
        }

        
        [HttpGet]
        public IActionResult GetStudents(string orderBy)
        {
            var result = from st in dbContext.Student
                         select new
                         {
                             st.IndexNumber,
                             st.FirstName,
                             st.LastName,
                             st.BirthDate,
                             st.IdEnrollment
                         };

            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetStudent(string id)
        {
            var student = (from st in dbContext.Student
                          where st.IndexNumber == id
                          select new
                          {
                              st.IndexNumber,
                              st.FirstName,
                              st.LastName,
                              st.BirthDate,
                              st.IdEnrollment
                          }).FirstOrDefault();

            if (student != null)
            {
                return Ok(student);
            }
            else
            {
                return NotFound("Student not found");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateStudent(string id, Student updatedStudent)
        {
                var student = (
                from st in dbContext.Student
                where st.IndexNumber == id
                select st).FirstOrDefault();

            student.FirstName = updatedStudent.FirstName;
            student.LastName = updatedStudent.LastName;
            student.BirthDate = updatedStudent.BirthDate;

            if (updatedStudent.Password != null)
            {
                student.Password = BCrypt.Net.BCrypt.HashPassword(updatedStudent.Password);
            }
    

            dbContext.Entry(student).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            dbContext.SaveChanges();

            return Ok(new
            {
                student.IndexNumber,
                student.FirstName,
                student.LastName,
                student.BirthDate,
                student.IdEnrollment
            });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(string id)
        {
            var student = (
                from st in dbContext.Student
                where st.IndexNumber == id
                select st).FirstOrDefault();

            if (student != null)
            {
                dbContext.Student.Remove(student);
                dbContext.SaveChanges();
            }

            return Ok("Usuwanie ukończone");
        }

        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            return Ok(student);
        }
    }
}
