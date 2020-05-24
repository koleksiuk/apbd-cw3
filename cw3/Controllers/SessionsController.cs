using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using cw3.DAL;
using cw3.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Linq;

namespace cw3.Controllers
{
    [ApiController]
    [Route("api/sessions")]
    public class SessionsController : ControllerBase
    {
        public IConfiguration Configuration { get; set; }
        public readonly UniversityContext dbContext;

        public SessionsController(UniversityContext context, IConfiguration configuration)
        {
            dbContext = context;
            Configuration = configuration;
        }

        [HttpPost("refresh")]
        public IActionResult RefreshToken(RefreshTokenRequest request)
        {
            var student = (
                from st in dbContext.Student
                where st.RefreshToken == request.RefreshToken
                select st
            ).FirstOrDefault();

            if (student == null)
            {
                return Unauthorized();
            }

            JwtSecurityToken token = generateJwtToken(student);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = request.RefreshToken
            });
        }

        [HttpPost()]
        public IActionResult SignIn(SignInRequest request)
        {
            var student = dbContext.Student.Find(request.IndexNumber);

            if (student == null)
            {
                return Unauthorized();
            }

            if (!student.IsValidPassword(request.Password))
            {
                return Unauthorized();
            }
            
            JwtSecurityToken token = generateJwtToken(student);

            var refreshToken = Guid.NewGuid();

            student.RefreshToken = refreshToken.ToString();
            dbContext.Student.Update(student);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken
            });
        }

        private JwtSecurityToken generateJwtToken(Student student)
        {
            var claims = new[]
                        {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, student.IndexNumber),
                new Claim(ClaimTypes.Role, "student"),
                new Claim(ClaimTypes.Role, "admin")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "Gakko",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                audience: "Students",
                signingCredentials: creds
            );

            return token;
        }
    }
}
