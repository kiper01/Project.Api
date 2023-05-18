using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Project.Core.Entities
{
    public class Teams
    {
        [Key]
        public Guid Id { get; set; }
        public string TeamsName { get; set; }
        [MaxLength(255)]
        public string TeamsPasswd { get; set; }

    }
}
