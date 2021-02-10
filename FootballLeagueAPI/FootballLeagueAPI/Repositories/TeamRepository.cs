using FootballLeagueAPI.DBContexts;
using FootballLeagueAPI.Models;

namespace FootballLeagueAPI.Repositories
{
    public class TeamRepository : RepositoryBase<Team>, ITeamRepository
    {
        public TeamRepository(SqlDbContext repositoryContext)
    : base(repositoryContext)
        {
        }
    }
}
