using AutoMapper;
using Praetura_demo.Entities;
using Praetura_demo.Models.LoanApplications;

namespace Praetura_demo.Profiles
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
