using AutoMapper;
using praetura_demo.Entities;
using praetura_demo.Models.DecisionLogEntries;

namespace praetura_demo.Profiles
{
    public class DecisionLogEntryProfile : Profile
    {
        public DecisionLogEntryProfile()
        {
            CreateMap<DecisionLogEntry, DecisionLogEntryDto>();
        }
    }
}
