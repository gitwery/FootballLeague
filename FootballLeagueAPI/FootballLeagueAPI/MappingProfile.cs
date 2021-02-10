using AutoMapper;
using FootballLeagueAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballLeagueAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MatchCreateUpdateDto, Match>();
            CreateMap<TeamCreateUpdateDto, Team>();
        }
    }
}
