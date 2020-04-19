﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using cw3.Models;

namespace cw3.DAL.MsSql
{
    public class StudentDbService : BaseDbService, IStudentDbService
    {
        public IEnumerable<Student> GetStudents()
        {
            var students = new List<Student>();

            using var com = new SqlCommand
            {
                Connection = Connection,
                CommandText = "SELECT * FROM Student"
            };

            Connection.Open();

            var dr = com.ExecuteReader();

            while (dr.Read())
            {
                var st = new Student();
                st.FirstName = dr["FirstName"].ToString();
                st.LastName = dr["LastName"].ToString();
                st.IndexNumber = dr["IndexNumber"].ToString();
                students.Add(st);
            }

            Connection.Close();

            return students;
        }

        public Student GetStudent(string id)
        {
            using var com = new SqlCommand
            {
                Connection = Connection,
                CommandText = @"
SELECT TOP 1 * FROM Student s
LEFT JOIN Enrollment e ON s.IdEnrollment = e.IdEnrollment
LEFT JOIN Studies st ON e.IdStudy = st.IdStudy
WHERE s.IndexNumber=@indexNumber;"
            };
            com.Parameters.AddWithValue("indexNumber", id);

            Connection.Open();

            try
            {
                var dr = com.ExecuteReader();

                while (dr.Read())
                {
                    Student st = new Student
                    {
                        FirstName = dr["FirstName"].ToString(),
                        LastName = dr["LastName"].ToString(),
                        IndexNumber = dr["IndexNumber"].ToString(),
                        BirthdateString = dr["Birthdate"].ToString(),
                        Enrollment = new Enrollment()
                    };
                    st.Enrollment.Semester = (int)dr["Semester"];
                    st.Enrollment.Study.Name = dr["Name"].ToString();

                    return st;
                }
            }
            catch (Exception e) {                
                throw e;
            } finally
            {
                Connection.Close();
            }

            

            return null;
        }
    }
}