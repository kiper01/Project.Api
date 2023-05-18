using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project.Core.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string Login { get; set; }
        [MaxLength(255)]
        public string Passwd { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

    }
}
