using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class AddImageDTO
    {
        [Required]
        public IFormFile Image { get; set; }
    }
}
