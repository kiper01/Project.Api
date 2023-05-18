using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Project.Core.Entities;
using Project.Core.Helpers;
using Project.Core.Models.Dto;
using Project.Core.Models.Request;
using Project.Core.Models.Response;
using Project.Core.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/v{v:apiVersion}/item")]
    [ApiController, ApiVersion("1")]
    [Authorize]
    public class ItemController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IItemService _itemService;
        private readonly ILogger _logger;

        public ItemController(IMapper mapper, IItemService itemService, ILogger<ItemController> logger)
        {
            _mapper = mapper;
            _itemService = itemService;
            _logger = logger;
        }

    }
}
