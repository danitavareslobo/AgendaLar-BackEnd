using AgendaLarAPI.Models.People.ViewModels;
using AutoMapper;

using Model = AgendaLarAPI.Models.People;

namespace AgendaLarAPI.Mappers.Phone
{
    public class CreatePhoneProfile : Profile
    {
        public CreatePhoneProfile()
        {
            CreateMap<CreatePhone, Model.Phone>()
                .ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.PersonId))
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Number))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type));
        }
    }
}
