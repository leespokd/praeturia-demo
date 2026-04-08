using AutoMapper;
using praetura_demo.Entities;
using praetura_demo.Models.LoanApplications;

namespace praetura_demo.Profiles
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
