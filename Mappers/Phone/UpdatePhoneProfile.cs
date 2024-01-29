using AgendaLarAPI.Models.People.ViewModels;
using AutoMapper;

using Model = AgendaLarAPI.Models.People;

namespace AgendaLarAPI.Mappers.Phone
{
    public class UpdatePhoneProfile : Profile
    {
        public UpdatePhoneProfile()
        {
            CreateMap<UpdatePhone, Model.Phone>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id)))
                .ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => Guid.Parse(src.PersonId)))
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Number))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.IsValid, src => src.Ignore())
                .ForMember(dest => dest.IsActive, src => src.Ignore())
                .ForMember(dest => dest.IsDeleted, src => src.Ignore())
                .ForMember(dest => dest.ValidationResult, src => src.Ignore());

            CreateMap<Model.Phone, UpdatePhone>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.PersonId))
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Number))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type));
        }
    }
}
