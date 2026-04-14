using AutoMapper;
using Praetura_demo.Entities;
using Praetura_demo.Models.DecisionLogEntries;

namespace Praetura_demo.Profiles
{
    public class DecisionLogEntryProfile : Profile
    {
        public DecisionLogEntryProfile()
        {
            CreateMap<DecisionLogEntry, DecisionLogEntryDto>();
        }
    }
}
