using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Core.Models.Dto.Programs
{
    public class GetProgramsRequest
    {
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
    }
}
