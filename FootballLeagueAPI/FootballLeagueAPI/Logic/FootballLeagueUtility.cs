using FootballLeagueAPI.Models;
using FootballLeagueAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballLeagueAPI.Logic
{
    public class FootballLeagueUtility

    {
        private IRepositoryWrapper _repository;

        public FootballLeagueUtility(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        public IQueryable<Match> GetAllMatches()
        {
            return _repository.Match.GetAll();
        }

        public IQueryable<Team> GetAllTeams()
        {
            return _repository.Team.GetAll();
        }

        public IQueryable<Team> GetAllTeamsRanked()
        {
            return GetAllTeams().OrderBy(t => t.Position);
        }

        public Match GetMatchById(int id)
        {
            return _repository.Match.GetByCondition(m => m.Id == id).FirstOrDefault();
        }

        public Team GetTeamById(int id)
        {
            return _repository.Team.GetByCondition(m => m.Id == id).FirstOrDefault();
        }

        public Team GetTeamByName(string teamName)
        {
            return _repository.Team.GetByCondition(team => team.Name.Equals(teamName)).FirstOrDefault();
        }

        public void CreateMatch(Match match, Team homeTeam, Team awayTeam)
        {
            _repository.Match.Create(match);
            UpdateTeamStatsOnCreateMatch(match, homeTeam, awayTeam);
            UpdatePositions();
            _repository.Save();
        }

        public void CreateTeam(Team team)
        {
            _repository.Team.Create(team);
            UpdatePositions();
            _repository.Save();
        }

        public void UpdateTeam(Team team)
        {
            _repository.Team.Update(team);

            var homeTeamMatches = _repository.Match.GetByCondition(m => m.HomeTeam.Equals(team.Name));
            foreach (var match in homeTeamMatches)
            {
                match.HomeTeam = team.Name;
            }

            var awayTeamMatches = _repository.Match.GetByCondition(m => m.AwayTeam.Equals(team.Name));
            foreach (var match in awayTeamMatches)
            {
                match.AwayTeam = team.Name;
            }

            UpdatePositions();
            _repository.Save();
        }

        public void UpdateMatch(Match match, Team homeTeam, Team awayTeam)
        {
            _repository.Match.Update(match);
            UpdateTeamStatsOnDeleteMatch(match, homeTeam, awayTeam);
            UpdateTeamStatsOnCreateMatch(match, homeTeam, awayTeam);
            UpdatePositions();
            _repository.Save();
        }

        public void DeleteMatch(Match match, Team homeTeam, Team awayTeam)
        {
            _repository.Match.Delete(match);
            UpdateTeamStatsOnDeleteMatch(match, homeTeam, awayTeam);
            UpdatePositions();
            _repository.Save();
        }

        public void DeleteTeam(Team team)
        {
            _repository.Team.Delete(team);

            var homeTeamMatches = _repository.Match.GetByCondition(m => m.HomeTeam.Equals(team.Name));
            foreach (var match in homeTeamMatches)
            {
                var awayTeam = GetTeamByName(match.AwayTeam);
                _repository.Match.Delete(match);
                UpdateTeamStatsOnDeleteMatch(match, team, awayTeam);
            }

            var awayTeamMatches = _repository.Match.GetByCondition(m => m.AwayTeam.Equals(team.Name));
            foreach (var match in awayTeamMatches)
            {
                var homeTeam = GetTeamByName(match.HomeTeam);
                _repository.Match.Delete(match);
                UpdateTeamStatsOnDeleteMatch(match, homeTeam, team);
            }

            UpdatePositions();
            _repository.Save();
        }

        internal void UpdateTeamStatsOnDeleteMatch(Match match, Team homeTeam, Team awayTeam)
        {
            homeTeam.GoalsScored -= match.HomeGoals;
            homeTeam.GoalsRecieved -= match.AwayGoals;
            homeTeam.GoalDifference -= match.HomeGoals - match.AwayGoals;

            awayTeam.GoalsScored -= match.AwayGoals;
            awayTeam.GoalsRecieved -= match.HomeGoals;
            awayTeam.GoalDifference -= match.AwayGoals - match.HomeGoals;

            if (match.MatchResult == MatchResult.HomeWin)
            {
                homeTeam.Points -= 3;
                homeTeam.Wins--;
                awayTeam.Loses--;
            }
            else if (match.MatchResult == MatchResult.AwayWin)
            {
                awayTeam.Points -= 3;
                awayTeam.Wins--;
                homeTeam.Loses--;
            }
            else
            {
                awayTeam.Points--;
                homeTeam.Points--;
                awayTeam.Draws--;
                homeTeam.Draws--;
            }
        }

        private void UpdateTeamStatsOnCreateMatch(Match match, Team homeTeam, Team awayTeam)
        {
            homeTeam.GoalsScored += match.HomeGoals;
            homeTeam.GoalsRecieved += match.AwayGoals;
            homeTeam.GoalDifference += match.HomeGoals - match.AwayGoals;

            awayTeam.GoalsScored += match.AwayGoals;
            awayTeam.GoalsRecieved += match.HomeGoals;
            awayTeam.GoalDifference += match.AwayGoals - match.HomeGoals;

            if (match.HomeGoals > match.AwayGoals)
            {
                match.MatchResult = MatchResult.HomeWin;
                homeTeam.Points += 3;
                homeTeam.Wins++;
                awayTeam.Loses++;
            }
            else if (match.HomeGoals < match.AwayGoals)
            {
                match.MatchResult = MatchResult.AwayWin;
                awayTeam.Points += 3;
                awayTeam.Wins++;
                homeTeam.Loses++;
            }
            else
            {
                match.MatchResult = MatchResult.Draw;
                awayTeam.Points++;
                homeTeam.Points++;
                awayTeam.Draws++;
                homeTeam.Draws++;
            }
        }

        private void UpdatePositions()
        {
            var teamsOrdered = _repository.Team.GetAll()
                .OrderByDescending(team => team.Points)
                .ThenByDescending(team => team.GoalDifference)
                .ThenByDescending(team => team.GoalsScored)
                .ToList();

            for (int i = 0; i < teamsOrdered.Count; i++)
            {
                _repository.Team.GetByCondition(team => teamsOrdered[i].Id.Equals(team.Id)).FirstOrDefault().Position = i + 1;
            }
        }

    }
}
