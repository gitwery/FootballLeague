using AutoMapper;
using FootballLeagueAPI.DBContexts;
using FootballLeagueAPI.Logic;
using FootballLeagueAPI.Models;
using FootballLeagueAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace FootballLeagueAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeamController : ControllerBase
    {

        private IMapper _mapper;
        private FootballLeagueUtility _utility;

        public TeamController(IRepositoryWrapper repository, IMapper mapper)
        {
            _mapper = mapper;
            _utility = new FootballLeagueUtility(repository);
        }

        [HttpGet("Ranking")]
        public IActionResult Get()
        {
            try
            {
                var teamsRanked = _utility.GetAllTeamsRanked().ToList();
                return Ok(teamsRanked);
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}", Name = "TeamById")]
        public IActionResult TeamById([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model state.");
                }

                var team = _utility.GetTeamById(id);

                if (team == null)
                {
                    return NotFound();
                }

                return Ok(team);
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public IActionResult AddTeam([FromBody] TeamCreateUpdateDto team)
        {
            try
            {
                if (team == null)
                {
                    return BadRequest("Match cannot be null.");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model state.");
                }

                var teamEntity = _mapper.Map<Team>(team);
                _utility.CreateTeam(teamEntity);

                return CreatedAtAction("TeamByID", new { id = teamEntity.Id }, teamEntity);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] TeamCreateUpdateDto team)
        {
            try
            {
                if (team == null)
                {
                    return BadRequest("Match object is null.");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model state.");
                }

                var teamEntity = _utility.GetTeamById(id);

                if (teamEntity == null)
                {
                    return NotFound();
                }

                _mapper.Map(team, teamEntity);
                _utility.UpdateTeam(teamEntity);

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model state.");
                }

                var team = _utility.GetTeamById(id);

                if (team == null)
                {
                    return NotFound();
                }

                _utility.DeleteTeam(team);
                return NoContent();

            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
