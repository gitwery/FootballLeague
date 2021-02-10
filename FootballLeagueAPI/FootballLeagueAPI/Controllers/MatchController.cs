using AutoMapper;
using FootballLeagueAPI.DBContexts;
using FootballLeagueAPI.Logic;
using FootballLeagueAPI.Models;
using FootballLeagueAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FootballLeagueAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MatchController : ControllerBase
    {

        private IMapper _mapper;
        private FootballLeagueUtility _utility;

        public MatchController(IRepositoryWrapper repository, IMapper mapper)
        {
            _mapper = mapper;
            _utility = new FootballLeagueUtility(repository);
        }

        [HttpGet]
        public IActionResult Matches()
        {
            try
            {
                var matches = _utility.GetAllMatches().ToList();
                return Ok(matches);
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}", Name = "MatchById")]
        public IActionResult MatchById([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model state.");
                }

                var match = _utility.GetMatchById(id);

                if (match == null)
                {
                    return NotFound();
                }

                return Ok(match);
            }
            catch
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        public IActionResult AddMatch([FromBody] MatchCreateUpdateDto match)
        {
            try
            {
                if (match == null)
                {
                    return BadRequest("Match cannot be null.");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model state.");
                }

                var homeTeam = _utility.GetTeamByName(match.HomeTeam);
                var awayTeam = _utility.GetTeamByName(match.HomeTeam);

                if (homeTeam == null || awayTeam == null)
                {
                    return BadRequest("No such pair of teams.");
                }

                var matchEntity = _mapper.Map<Match>(match);
                _utility.CreateMatch(matchEntity, homeTeam, awayTeam);

                return CreatedAtRoute("MatchById", new { id = matchEntity.Id }, matchEntity);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateMatch([FromRoute] int id, [FromBody] MatchCreateUpdateDto match)
        {
            try
            {
                if (match == null)
                {
                    return BadRequest("Match object is null.");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model state.");
                }

                var matchEntity = _utility.GetMatchById(id);
                var homeTeam = _utility.GetTeamByName(match.HomeTeam);
                var awayTeam = _utility.GetTeamByName(match.HomeTeam);

                if (matchEntity == null || homeTeam == null || awayTeam == null)
                {
                    return NotFound();
                }

                _mapper.Map(match, matchEntity);
                _utility.UpdateMatch(matchEntity, homeTeam, awayTeam);

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }


            //return new OkObjectResult("Match updated.");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMatch([FromRoute] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model state.");
                }

                var match = _utility.GetMatchById(id);
                var homeTeam = _utility.GetTeamByName(match.HomeTeam);
                var awayTeam = _utility.GetTeamByName(match.HomeTeam);

                if (match == null || homeTeam == null || awayTeam == null)
                {
                    return NotFound();
                }

                _utility.DeleteMatch(match, homeTeam, awayTeam);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
