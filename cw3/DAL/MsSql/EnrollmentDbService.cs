using System;
using System.Data.SqlClient;
using cw3.DTOs.Requests;
using cw3.DTOs.Responses;
using static cw3.DAL.IEnrollmentDbService;

namespace cw3.DAL.MsSql
{
    public class EnrollmentDbService : BaseDbService, IEnrollmentDbService
    {
        public EnrollStudentResponse EnrollStudent(EnrollStudentRequest request)
        {
            Connection.Open();

            SqlCommand command = Connection.CreateCommand();

            var startDate = Convert.ToDateTime("01-09-2020"); // hardcoded

            var tran = Connection.BeginTransaction();

            command.Connection = Connection;
            command.Transaction = tran;

            try
            {
                command.CommandText = "SELECT TOP 1 IdStudy FROM Studies where name=@name";
                command.Parameters.AddWithValue("name", request.Studies);

                var dr = command.ExecuteReader();

                if (!dr.Read())
                {
                    dr.Close();
                    tran.Rollback();
                    Connection.Close();

                    throw new OperationException("Studies not found");
                }

                int studyId = (int)dr["IdStudy"];

                dr.Close();

                command.CommandText = @"
INSERT INTO Enrollment(IdEnrollment, Semester, IdStudy, StartDate)
OUTPUT INSERTED.IdEnrollment
VALUES((select max(IdEnrollment)+1 from Enrollment), 1, @idStudy, @startDate);";
                command.Parameters.AddWithValue("idStudy", studyId);
                command.Parameters.AddWithValue("startDate", Convert.ToString(startDate));

                int enrollmentId = (int)command.ExecuteScalar(); // returns ID of enrollment

                command.CommandText = "SELECT TOP 1 IndexNumber FROM Student where IndexNumber=@indexNumber";
                command.Parameters.AddWithValue("indexNumber", request.IndexNumber);

                dr = command.ExecuteReader();

                if (dr.Read())
                {
                    dr.Close();
                    tran.Rollback();
                    Connection.Close();

                    throw new OperationException("Student already exists");
                }
                dr.Close();


                command.CommandText = @"
INSERT INTO Student(IndexNumber, FirstName, LastName, BirthDate, IdEnrollment)
VALUES(@indexNumber, @firstName, @lastName, @birthdate, @idEnrollment)";
                command.Parameters.AddWithValue("firstName", request.FirstName);
                command.Parameters.AddWithValue("lastName", request.LastName);
                command.Parameters.AddWithValue("birthdate", request.Birthdate);
                command.Parameters.AddWithValue("idEnrollment", enrollmentId);

                command.ExecuteNonQuery();

                tran.Commit();

                Connection.Close();

                EnrollStudentResponse response = new EnrollStudentResponse
                {
                    LastName = request.LastName,
                    Semester = 1,
                    StartDate = startDate,
                    EnrollmentId = enrollmentId
                };

                return response;
            }
            catch (SqlException ex)
            {                
                tran.Rollback();
                Connection.Close();
                throw new OperationException($"Unknown exception: {ex.Message}");
            }

            
        }

        public void PromoteStudents(int semester, string studies)
        {
            throw new NotImplementedException();
        }
    }
}
