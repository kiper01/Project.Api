using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Project.Core.Entities;
using Project.Core.Helpers;
using Project.Core.Models.Dto;
using Project.Core.Models.Dto.User;
using Project.Core.Models.Request;
using Project.Core.Models.Response;
using Project.Core.OperationInterfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/v{v:apiVersion}/user")]
    [ApiController, ApiVersion("1")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly ILogger _logger;

        public UsersController(IMapper mapper, IUserService userService, ILogger<ItemController> logger)
        {
            _mapper = mapper;
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("get/me")]
        public ResultRequest<DtoUser> GetMe([FromBody] InboundRequest request)
        {
            try
            {
                var userid = User.GetCurrentUserId();

                var user = _userService.Get<User>(userid);

                var dtoUser = _mapper.Map<DtoUser>(user);

                return ResultRequest<DtoUser>.Ok(dtoUser);
            }
            catch (Exception e)
            {
                _logger.LogError("Get elements User error", e.ToString());
                return ResultRequest<DtoUser>.Error("Error", e.ToString());
            }
        }

        [HttpPost("update-my-user")]
        public async Task<ResultRequest> UpdateMyUser([FromBody] InboundRequest<DtoEditMyUser> request)
        {
            using (var transaction = _userService.BeginTransaction())
            {
                try
                {
                    var dto = request?.Data;
                    if (dto == null)
                    {
                        await transaction.RollbackAsync();
                        return ResultRequest.Error("Error", "Invalid request data");
                    }
                    var dbUser = _userService.Get<User>(User.GetCurrentUserId());

                    if (dbUser == null)
                    {
                        await transaction.RollbackAsync();
                        return ResultRequest.Error("Error", "Wrong User");
                    }
                    if (!String.IsNullOrEmpty(dto.NewPassword))
                    {
                        _userService.UpdateUser(dbUser, dto.NewPassword);
                    }
                    else
                    {
                        _userService.UpdateUser(dbUser);
                    }
                    await transaction.CommitAsync();
                    return ResultRequest.Ok();
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError("Update element User error", e.ToString());
                    return ResultRequest.Error("Error", e.ToString());
                }
            }
        }

        [HttpPost("get/{id}")]
        public ResultRequest<DtoEditUser> Get(Guid id)
        {
            try
            {

                var user = _userService.Get<User>(id);
                var dtoUser = _mapper.Map<DtoEditUser>(user);
                return ResultRequest<DtoEditUser>.Ok(dtoUser);
            }
            catch (Exception e)
            {
                _logger.LogError("Get elements User error", e.ToString());
                return ResultRequest<DtoEditUser>.Error("Error", e.ToString());
            }
        }

        [HttpGet("get-all")]
        public async Task<ResultRequest<List<DtoEditUser>>> GetAll()
        {
            try
            {
                return ResultRequest<List<DtoEditUser>>.Ok(_mapper.Map<List<DtoEditUser>>(await _userService.GetAllAsync<User>()));
            }
            catch (Exception e)
            {
                _logger.LogError("Get all elements User error", e.ToString());
                return ResultRequest<List<DtoEditUser>>.Error("Get all elements user error", e.Message);
            }
        }
        [Authorize(Roles = "admin")]
        [HttpPost("delete/{id}")]
        public async Task<ResultRequest> Delete(Guid id)
        {
            using (var transaction = _userService.BeginTransaction())
            {
                try
                {
                    var dbUser = await _userService.GetAsync<User>(id);
                    _userService.Delete(dbUser);
                    await transaction.CommitAsync();
                    return ResultRequest.Ok();
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError("Deleting element User error", e.ToString());
                    return ResultRequest.Error("Deleting element User error", e.Message);
                }
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost("update")]
        public async Task<ResultRequest> Update([FromBody] InboundRequest<DtoEditUser> request)
        {
            using (var transaction = _userService.BeginTransaction())
            {
                try
                {
                    var dto = request?.Data;
                    if (dto == null)
                    {
                        await transaction.RollbackAsync();
                        return ResultRequest.Error("Error", "Invalid request data");
                    }
                    var dbUser = _userService.Get<User>(dto.Id);

                    if (dbUser == null)
                    {
                        await transaction.RollbackAsync();
                        return ResultRequest.Error("Error", "Wrong User");
                    }
                    _mapper.Map(dto, dbUser);
                    if (!String.IsNullOrEmpty(dto.NewPassword))
                    {
                        _userService.UpdateUser(dbUser, dto.NewPassword);
                    }
                    else
                    {
                        _userService.UpdateUser(dbUser);
                    }
                    await transaction.CommitAsync();
                    return ResultRequest.Ok();
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError("Update element User error", e.ToString());
                    return ResultRequest.Error("Error", e.ToString());
                }
            }
        }
        [Authorize(Roles = "admin")]
        [HttpPost("add")]
        public async Task<ResultRequest<DtoEditUser>> Add([FromBody] InboundRequest<DtoEditUser> request)
        {
            using (var transaction = _userService.BeginTransaction())
            {
                try
                {
                    var dto = request?.Data;
                    if (dto == null)
                    {
                        await transaction.RollbackAsync();
                        return ResultRequest<DtoEditUser>.Error("Adding element failed", "Invalid request data");
                    }

                    var user = _mapper.Map<User>(dto);

                    var addedUser = _userService.Add(user, dto.NewPassword);
                    var mappedUser = _mapper.Map<DtoEditUser>(addedUser);
                    await transaction.CommitAsync();
                    return ResultRequest<DtoEditUser>.Ok(mappedUser);
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError("Update element User error", e.ToString());
                    return ResultRequest<DtoEditUser>.Error("Adding element error", e.Message);
                }
            }
        }
    }
}
