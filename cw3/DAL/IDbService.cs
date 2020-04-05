﻿using System;
using System.Collections.Generic;
using cw3.Models;

namespace cw3.DAL
{
    public interface IDbService
    {
        public IEnumerable<Student> GetStudents();
        public Student GetStudent(string id);
    }
}
