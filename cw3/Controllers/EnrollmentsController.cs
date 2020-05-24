using cw3.DAL;
using Microsoft.AspNetCore.Mvc;
using cw3.DTOs.Requests;
using static cw3.DAL.IEnrollmentDbService;
using Microsoft.AspNetCore.Authorization;

namespace cw3.Controllers
{
    [ApiController]
    [Route("api/enrollments")]
    [Authorize(Roles = "employee")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IEnrollmentDbService _dbService;

        public EnrollmentsController(IEnrollmentDbService dbService)
        {
            _dbService = dbService;
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
            try
            {
                var response = _dbService.EnrollStudent(request);

                return Created("/enrollments/1", response);
            }
            catch (OperationException ex)
            {
                return BadRequest(ex.Message);
            }            
        }

        [HttpPut("promotions")]
        public IActionResult PromoteStudents(PromoteStudentsRequest request)
        {
            _dbService.PromoteStudents(request.Semester, request.Studies);

            return Created("/enrollments/", request);
        }
    }
}
