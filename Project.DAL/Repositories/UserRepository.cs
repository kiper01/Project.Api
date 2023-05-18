using Microsoft.EntityFrameworkCore;
using Project.Core.Entities;
using Project.Core.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Project.DAL.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ProjectDbContext context) : base(context)
        {
        }
        public ProjectDbContext PrognozDbContext => Context as ProjectDbContext;

    }
}
