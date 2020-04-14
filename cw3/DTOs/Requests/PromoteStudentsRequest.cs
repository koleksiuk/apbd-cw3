using System;
using System.ComponentModel.DataAnnotations;

namespace cw3.DTOs.Requests
{
    public class PromoteStudentsRequest
    {
        public PromoteStudentsRequest()
        {
        }

        [Required]
        public int Semester { get; set; }

        [Required]
        public string Studies { get; set; }
    }
}
