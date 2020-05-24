using System;
using System.ComponentModel.DataAnnotations;

namespace cw3.DTOs.Requests
{
    public class RefreshTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
