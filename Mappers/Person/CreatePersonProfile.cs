using AgendaLarAPI.Models.People.ViewModels;
using AutoMapper;

using Model = AgendaLarAPI.Models.People;

namespace AgendaLarAPI.Mappers.Person
{
    public class CreatePersonProfile : Profile
    {
        public CreatePersonProfile()
        {
            CreateMap<CreatePerson, Model.Person>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.SocialNumber, opt => opt.MapFrom(src => src.SocialNumber))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
                .ForMember(dest => dest.Phones, opt => opt.MapFrom(src => src.Phones));
        }
    }
}
