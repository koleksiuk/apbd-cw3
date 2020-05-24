using System;
using System.Collections.Generic;


namespace cw3
{
    public partial class Student
    {
        public string IndexNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Password { get; set; }
        public DateTime BirthDate { get; set; }
        public int IdEnrollment { get; set; }

        public string RefreshToken { get; set; }

        public virtual Enrollment IdEnrollmentNavigation { get; set; }

        public bool IsValidPassword(string pass)
        {
            return BCrypt.Net.BCrypt.Verify(pass, Password);
        }
    }
}
