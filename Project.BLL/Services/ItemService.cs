using Microsoft.Extensions.Configuration;
using Project.Core.RepositoryInterfaces;
using Project.Core.ServiceInterfaces;

namespace Project.BLL.Services
{
    public class ItemService : ServiceBase, IItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public ItemService(IUnitOfWork unitOfWork, IConfiguration configuration) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
    }
}

