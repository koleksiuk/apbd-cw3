using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using cw3.Models;

namespace cw3.DAL
{
    public class MsSqlDbService : IDbService
    {
        private string _connectionDetails =
            "Server=localhost,1433;Database=university;User Id=SA;Password=Pass@word;";

        SqlConnection Connection;

        public MsSqlDbService()
        {
            Connection = new SqlConnection(_connectionDetails);
        }

        public IEnumerable<Student> GetStudents()
        {
            var students = new List<Student>();

            using var com = new SqlCommand();
            com.Connection = Connection;
            com.CommandText = "SELECT * FROM Student";

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
    }
}
