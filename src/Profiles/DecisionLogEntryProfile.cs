using AutoMapper;
using Praetoria_demo.Entities;
using Praetoria_demo.Models.DecisionLogEntries;

namespace Praetoria_demo.Profiles
{
    public class DecisionLogEntryProfile : Profile
    {
        public DecisionLogEntryProfile()
        {
            CreateMap<DecisionLogEntry, DecisionLogEntryDto>();
        }
    }
}
