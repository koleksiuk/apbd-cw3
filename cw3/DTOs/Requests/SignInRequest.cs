using System;
using System.ComponentModel.DataAnnotations;

namespace cw3.DTOs.Requests
{
    public class SignInRequest
    {
        [Required]
        public string IndexNumber { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
