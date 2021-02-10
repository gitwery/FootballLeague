using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FootballLeagueAPI.Models
{
    public class MatchCreateUpdateDto
    {
        [Required(ErrorMessage = "Home team is required")]
        [StringLength(50, ErrorMessage = "Home team cannnot be longer than 50 characters")]
        public string HomeTeam { get; set; }

        [Required(ErrorMessage = "Away team is required")]
        [StringLength(50, ErrorMessage = "Away team cannnot be longer than 50 characters")]
        public string AwayTeam { get; set; }

        [Required(ErrorMessage = "Home team goals required")]
        public int HomeGoals { get; set; }

        [Required(ErrorMessage = "Away team goals required")]
        public int AwayGoals { get; set; }
    }
}
