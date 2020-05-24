using System;
using System.Collections.Generic;
using cw3.Models;

namespace cw3.DAL
{
    public class MockDbService : IStudentDbService
    {
        private static readonly IEnumerable<Student> _students;

        static MockDbService()
        {
            _students = new List<Student>
            {
                new Student{FirstName="Jan", LastName="Kowalski", IndexNumber="s10000"},
                new Student{FirstName="Anna", LastName="Kowalska", IndexNumber="s12000"},
                new Student{FirstName="Paweł", LastName="Nowak", IndexNumber="s14000"}
            };
        }

        public IEnumerable<Student> GetStudents()
        {
            return _students;
        }

        public Student GetStudent(String id)
        {
            return (_students as List<Student>).Find(st => st.IndexNumber == id);            
        }


        public Student GetStudentForAuth(string indexNumber, string password)
        {
            return (_students as List<Student>).Find(st => st.IndexNumber == indexNumber);
        }
    }
}
