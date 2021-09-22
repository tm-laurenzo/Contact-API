
using Microsoft.AspNetCore.Identity;
using System;

namespace Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string ImageUrl { get; set; }
        public string PublicId { get; set; }
    }
}
