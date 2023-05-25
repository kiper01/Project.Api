using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Project.Core.Entities
{
    public class Programs
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string ProgramName { get; set; }
        public TimeSpan TimeStart { get; set; }
        public TimeSpan TimeEnd { get; set; }
        public TimeSpan TotalTime { get; set; }
        public DateTime Date { get; set; }

    }
}
