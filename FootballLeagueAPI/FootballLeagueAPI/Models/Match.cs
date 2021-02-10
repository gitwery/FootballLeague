﻿namespace FootballLeagueAPI.Models
{
    public class Match
    {
        public int Id { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public int HomeGoals { get; set; }
        public int AwayGoals { get; set; }
        public MatchResult MatchResult { get; set; }
    }
}
