using System;
using System.Linq;
using BuildMaster.Model;

namespace BuildMaster.Infrastructure
{
    public interface IRepository
    {
        void EnsureSeedData();
    }

    public class Repository : IRepository
    {
        private readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void EnsureSeedData()
        {
            if (_context.Configurations.Any())
                return;

            using (var trn = _context.Database.BeginTransaction())
            {
                _context.Configurations.AddRange(new[] {
                    new Configuration
                    {
                        Key = "version",
                        Value = "0.1"
                    },
                    new Configuration
                    {
                        Key = "banner",
                        Value = "Build Master"
                    },
                    new Configuration
                    {
                        Key = "temp path",
                        Value = "/temp"
                    },
                    new Configuration
                    {
                        Key = "tools path",
                        Value = "/environment"
                    },
                    new Configuration
                    {
                        Key = "log path",
                        Value = "/temp/logs"
                    },
                    new Configuration
                    {
                        Key = "status",
                        Value = "stopped"
                    }
                });
                _context.SaveChanges();

                trn.Commit();
            }
        }
    }
}