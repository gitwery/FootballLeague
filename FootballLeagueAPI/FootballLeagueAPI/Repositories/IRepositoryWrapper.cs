namespace FootballLeagueAPI.Repositories
{
    public interface IRepositoryWrapper
    {
        IMatchRepository Match { get; }
        ITeamRepository Team { get; }
        void Save();
    }
}