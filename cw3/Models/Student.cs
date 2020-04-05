using System;
namespace cw3.Models
{
    public class Student
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IndexNumber { get; set; }
        public DateTime Birthdate { get; set; }
        public Enrollment Enrollment { get; set; }

        public String BirthdateString
        {
            set
            {
                try
                {
                    this.Birthdate = Convert.ToDateTime(value);
                }
                catch (FormatException)
                {
                }
            }
            get
            {
                return this.Birthdate.ToString("dd.MM.yyyy");
            }
        }
    }
}
