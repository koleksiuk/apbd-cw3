using cw3.DAL;
using Microsoft.AspNetCore.Mvc;
using cw3.DTOs.Requests;
using static cw3.DAL.IEnrollmentDbService;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System;

namespace cw3.Controllers
{
    [ApiController]
    [Route("api/enrollments")]
    //[Authorize(Roles = "employee")]
    public class EnrollmentsController : ControllerBase
    {
        public readonly UniversityContext dbContext;

        public EnrollmentsController(UniversityContext context)
        {
            dbContext = context;
        }

        
        [HttpPost]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            /*
            {
              "Studies": "Maths",
              "firstname": "Jan",
              "lastname": "Kowalski",
              "birthdate": "1990-01-01T00:00:00",
              "indexnumber": "10021"
            }
            */

            Studies study = (from studies in dbContext.Studies
                         where studies.Name == request.Studies
                             select studies).FirstOrDefault();

            if (study == null)
            {
                return BadRequest("Studia nie znalezione");
            }

            Enrollment lastEnrollment = (from enrollments in dbContext.Enrollment
                                         orderby enrollments.IdEnrollment descending
                                         select enrollments).FirstOrDefault();

            int idEnrollment;

            if (lastEnrollment == null)
            {
                idEnrollment = 1;
            }
            else
            {
                idEnrollment = lastEnrollment.IdEnrollment + 1;
            }

            Enrollment en = new Enrollment
            {
                IdEnrollment = idEnrollment,
                IdStudy = study.IdStudy,
                StartDate = DateTime.Now
            };

            var studentExists = (from students in dbContext.Student
                                 where students.IndexNumber == request.IndexNumber
                                 select students).Any();

            if (studentExists)
            {
                return BadRequest($"Student z indeksem {request.IndexNumber} istnieje w bazie");
            }

            Student st = new Student
            {
                IndexNumber = request.IndexNumber,
                FirstName = request.FirstName,
                LastName = request.LastName,
                BirthDate = request.Birthdate,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                IdEnrollment = en.IdEnrollment
            };

            dbContext.Add(en);
            dbContext.Add(st);

            dbContext.SaveChanges();

            return Created($"/enrollments/{en.IdEnrollment}", new
            {
                en.IdEnrollment,
                st.IndexNumber
            });
        }

        [HttpPut("promotions")]
        public IActionResult PromoteStudents(PromoteStudentsRequest request)
        {
            Studies study = (from st in dbContext.Studies
                               where st.Name == request.Studies
                               select st).First();

            Enrollment thisEnrollment = (from en in dbContext.Enrollment
                                         where en.Semester == request.Semester
                                         where en.IdStudy == study.IdStudy
                                         select en).First();

            Enrollment nextEnrollment = (from en in dbContext.Enrollment
                                     where en.Semester == request.Semester + 1
                                     where en.IdStudy == study.IdStudy
                                     select en).FirstOrDefault();

            if (nextEnrollment == null)
            {
                Enrollment lastEnrollment = (from enrollments in dbContext.Enrollment
                                             orderby enrollments.IdEnrollment descending
                                             select enrollments).FirstOrDefault();

                int idEnrollment;

                if (lastEnrollment == null)
                {
                    idEnrollment = 1;
                }
                else
                {
                    idEnrollment = lastEnrollment.IdEnrollment + 1;
                }

                nextEnrollment = new Enrollment
                {
                    IdStudy = study.IdStudy,
                    Semester = request.Semester + 1,
                    IdEnrollment = idEnrollment,
                    StartDate = DateTime.Now
                };

                dbContext.Add(nextEnrollment);
            }

            var students = (from st in dbContext.Student
                            where st.IdEnrollment == thisEnrollment.IdEnrollment
                            select st);

            foreach (Student st in students)
            {
                st.IdEnrollment = nextEnrollment.IdEnrollment;
                dbContext.Add(st);
            }

            dbContext.SaveChanges();

            return Created("/enrollments/", request);
        }
    }
}
