using System;
using cw3.DTOs.Requests;
using cw3.DTOs.Responses;

namespace cw3.DAL
{
    public interface IEnrollmentDbService
    {
        public class OperationException : Exception
        {
            public OperationException(string message) : base(message)
            {
            }
        }

        EnrollStudentResponse EnrollStudent(EnrollStudentRequest request);
        void PromoteStudents(int semester, string studies);
    }
}
