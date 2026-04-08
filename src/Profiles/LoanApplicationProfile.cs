using AutoMapper;
using Praetoria_demo.Entities;
using Praetoria_demo.Models.LoanApplications;

namespace Praetoria_demo.Profiles
{
    public class LoanApplicationProfile : Profile
    {

        public LoanApplicationProfile()
        {
            CreateMap<LoanApplication, LoanApplicationDto>()
                .ForMember(dest => dest.DecisionLogEntries, opt => opt.MapFrom(src => src.DecisionLogEntries));
            CreateMap<CreateLoanApplicationDto, LoanApplication>()
                .ForMember(dest => dest.DecisionLogEntries, opt => opt.Ignore());
        }
    }
}
