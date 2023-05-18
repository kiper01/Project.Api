using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Project.Core.Entities;
using Project.Core.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Threading.Tasks;

namespace Project.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly ProjectDbContext _context;
        public IUserRepository User { get; }
        public ITeamsRepository Teams { get; }
        public IUserToTeamsRepository UserToTeams { get; }
        public IProgramsRepository Programs { get; }

        public UnitOfWork(ProjectDbContext context)
        {
            _context = context;

            User = new UserRepository(_context);
            Teams = new TeamsRepository(_context);
            UserToTeams = new UserToTeamsRepository(_context);
            Programs = new ProgramsRepository(_context);


        }

        public IRepository<T> Repository<T>()
            where T : class
        {
            if (typeof(T) == typeof(User))
            {
                return User as IRepository<T>;
            }
            else if (typeof(T) == typeof(Teams))
            {
                return Teams as IRepository<T>;
            }
            else if (typeof(T) == typeof(UserToTeams))
            {
                return UserToTeams as IRepository<T>;
            }
            else if (typeof(T) == typeof(Programs))
            {
                return Programs as IRepository<T>;
            }
            return null;
        }


        public int ExecuteSqlCommand(FormattableString _sql)
        {
            return _context.Database.ExecuteSqlInterpolated(_sql);
        }
        public Task<int> ExecuteSqlCommandAsync(FormattableString _sql)
        {
            return _context.Database.ExecuteSqlInterpolatedAsync(_sql);
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        public IEnumerable<dynamic> GetObjectsToSQL(string sql)
        {
            var resultSQLRequest = new List<dynamic>();
            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = sql;
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                using (var dataReader = cmd.ExecuteReader())
                {

                    while (dataReader.Read())
                    {
                        var dataRow = GetDataRow(dataReader);
                        resultSQLRequest.Add(dataRow);
                    }
                }
            }
            return resultSQLRequest;
        }
        private static dynamic GetDataRow(DbDataReader dataReader)
        {
            var dataRow = new ExpandoObject() as IDictionary<string, object>;
            for (var fieldCount = 0; fieldCount < dataReader.FieldCount; fieldCount++)
                dataRow.Add(dataReader.GetName(fieldCount), dataReader[fieldCount]);
            return dataRow;
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
