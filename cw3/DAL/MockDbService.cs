using System;
using System.Collections.Generic;
using cw3.Models;

namespace cw3.DAL
{
    public class MockDbService : IDbService
    {
        private static readonly IEnumerable<Student> _students;

        static MockDbService()
        {
            _students = new List<Student>
            {
                new Student{IdStudent=1, FirstName="Jan", LastName="Kowalski"},
                new Student{IdStudent=2, FirstName="Anna", LastName="Kowalska"},
                new Student{IdStudent=3, FirstName="Paweł", LastName="Nowak"}
            };
        }

        public IEnumerable<Student> GetStudents()
        {
            return _students;
        }

    }
}
