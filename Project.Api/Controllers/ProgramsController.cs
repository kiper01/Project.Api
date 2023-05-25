using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Project.Core.Entities;
using Project.Core.Helpers;
using Project.Core.Models.Dto;
using Project.Core.Models.Dto.Programs;
using Project.Core.Models.Dto.User;
using Project.Core.Models.Request;
using Project.Core.Models.Response;
using Project.Core.OperationInterfaces;
using Project.Core.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Project.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/v{v:apiVersion}/programs")]
    [ApiController, ApiVersion("1")]

    public class ProgramsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProgramsService _programsService;
        private readonly ILogger _logger;

        public ProgramsController(IMapper mapper, IProgramsService programsService, ILogger<ProgramsController> logger)
        {
            _mapper = mapper;
            _programsService = programsService;
            _logger = logger;
        }



        [HttpPost("get-all")]
        public async Task<ResultRequest<List<DtoGetProgramResponse>>> GetPrograms([FromBody] GetProgramsRequest request)
        {
            try
            {
                var programs = await _programsService.GetAllAsync<Programs>();

                // Фильтрация программ по UserID и полю Date
                programs = programs.Where(p => p.UserId == request.UserId && p.Date.Date == request.Date.Date).ToList();

                return ResultRequest<List<DtoGetProgramResponse>>.Ok(_mapper.Map<List<DtoGetProgramResponse>>(programs));
            }
            catch (Exception e)
            {
                _logger.LogError("Get all programs error", e.ToString());
                return ResultRequest<List<DtoGetProgramResponse>>.Error("Get all programs error", e.Message);
            }
        }

        [HttpPost("add")]
        public async Task<ResultRequest<DtoGetProgramResponse>> Add([FromBody] InboundRequest<DtoGetProgramResponse> request)
        {
            using (var transaction = _programsService.BeginTransaction())
            {
                try
                {
                    var dto = request?.Data;
                    if (dto == null)
                    {
                        await transaction.RollbackAsync();
                        return ResultRequest<DtoGetProgramResponse>.Error("Adding element failed", "Invalid request data");
                    }

                    var field = _mapper.Map<Programs>(dto);

                    var addedField = _programsService.Add(field);
                    var mappedField = _mapper.Map<DtoGetProgramResponse>(addedField);
                    await transaction.CommitAsync();
                    return ResultRequest<DtoGetProgramResponse>.Ok(mappedField);
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError("Update element User error", e.ToString());
                    return ResultRequest<DtoGetProgramResponse>.Error("Adding element error", e.Message);
                }
            }
        }

    }
}
