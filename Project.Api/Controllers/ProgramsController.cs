using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Project.Core.Entities;
using Project.Core.Helpers;
using Project.Core.Models.Dto;
using Project.Core.Models.Dto.Programs;
using Project.Core.Models.Request;
using Project.Core.Models.Response;
using Project.Core.OperationInterfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Project.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/v{v:apiVersion}/programs")]
    [ApiController, ApiVersion("1")]
    [Authorize]

        public class ProgramsController : ControllerBase
        {
            [HttpPost("getProgramResults")]
            public IActionResult GetProgramResults([FromBody] DtoGetProgramUseRequest request)
            {
                // ��������� ������ �� request.UserId � request.Date
                string userId = request.UserId;
                DateTime date = request.Date;

                // ������ ������� ����������� ��������� ��� ��������� ����
                List<DtoGetProgramResponse> programResults = GetProgramResultsForDate(userId, date);

                // ����������� ����������
                return Ok(programResults);
            }

            private List<DtoGetProgramResponse> GetProgramResultsForDate(string userId, DateTime date)
            {
                // ������ ������� ����������� ��������� ��� ��������� ����
                // ...

                // ������ �������� ������ �����������
                List<DtoGetProgramResponse> programResults = new List<DtoGetProgramResponse>();

               /* // ���������� ������� ���������� ��������� � ������
                DtoGetProgramResponse programResult = new DtoGetProgramResponse
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.Parse(userId),
                    ProgramName = "Example Program",
                    TimeStart = date.AddHours(9),
                    TimeEnd = date.AddHours(10),
                    TotalTime = TimeSpan.FromHours(1)
                };

                programResults.Add(programResult);

                // ... */

                return programResults;
            }






        }
}
