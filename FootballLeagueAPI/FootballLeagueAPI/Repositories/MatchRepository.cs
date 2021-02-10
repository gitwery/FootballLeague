using FootballLeagueAPI.DBContexts;
using FootballLeagueAPI.Models;

namespace FootballLeagueAPI.Repositories
{
    public class MatchRepository : RepositoryBase<Match>, IMatchRepository
    {
        public MatchRepository(SqlDbContext repositoryContext)
    : base(repositoryContext)
        {
        }
    }
}
