using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Core.Models.Dto.Programs
{
    public class DtoGetProgramResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string ProgramName { get; set; }
        public TimeSpan TimeStart { get; set; }
        public TimeSpan? TimeEnd { get; set; }
        public TimeSpan? TotalTime { get; set; }
        public DateTime Date { get; set; }
    }
}
