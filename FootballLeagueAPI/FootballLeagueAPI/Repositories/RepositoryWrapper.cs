using FootballLeagueAPI.DBContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballLeagueAPI.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private SqlDbContext _context;
        private IMatchRepository _match;
        private ITeamRepository _team;
        public IMatchRepository Match
        {
            get
            {
                if (_match == null)
                {
                    _match = new MatchRepository(_context);
                }
                return _match;
            }
        }
        public ITeamRepository Team
        {
            get
            {
                if (_team == null)
                {
                    _team = new TeamRepository(_context);
                }
                return _team;
            }
        }
        public RepositoryWrapper(SqlDbContext repositoryContext)
        {
            _context = repositoryContext;
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
