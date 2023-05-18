using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Project.Core.Entities
{
    public class Programs
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User user { get; set; }
        public string ProgramName { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public DateTime TotalTime { get; set; }
        public DateTime Date { get; set; }

    }
}
