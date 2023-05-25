using Microsoft.Extensions.Configuration;
using Project.Core.Entities;
using Project.Core.Enum;
using Project.Core.Exceptions;
using Project.Core.RepositoryInterfaces;
using Project.Core.ServiceInterfaces;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace Project.BLL.Services
{
    public class ProgramsService : ServiceBase, IProgramsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public ProgramsService(IUnitOfWork unitOfWork, IConfiguration configuration) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;

        }
       
    }
}

