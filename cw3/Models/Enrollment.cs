using System;
namespace cw3.Models
{
    public partial class Enrollment
    {
        public Enrollment() {
            Study = new Study();
        }
        
        public int IdEnrollment { get; set; }
        public int Semester { get; set; }
        public Study Study { get; set; }
        public DateTime StartDate { get; set; }
        
    }
}
