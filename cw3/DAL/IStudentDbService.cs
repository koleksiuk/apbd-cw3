using System.Collections.Generic;
using cw3.Models;

namespace cw3.DAL
{
    public interface IStudentDbService
    {
        public IEnumerable<Student> GetStudents();
        public Student GetStudent(string id);
        public void UpdateRefreshToken(Student student, string refreshToken);
        public Student GetStudentForRefreshToken(string refreshToken);
    }
}
