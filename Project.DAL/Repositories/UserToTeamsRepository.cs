using Project.Core.Entities;
using Project.Core.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.DAL.Repositories
{
    public class UserToTeamsRepository : Repository<UserToTeams>, IUserToTeamsRepository
    {
        public UserToTeamsRepository(ProjectDbContext context) : base(context)
        {
        }
        public ProjectDbContext PrognozDbContext => Context as ProjectDbContext;
    }
}
