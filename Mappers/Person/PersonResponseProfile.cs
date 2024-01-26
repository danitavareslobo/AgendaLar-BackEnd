using AgendaLarAPI.Models.Person.ViewModels;
using AutoMapper;

using Model = AgendaLarAPI.Models.Person;

namespace AgendaLarAPI.Mappers.Person
{
    public class PersonResponseProfile : Profile
    {
        public PersonResponseProfile()
        {
            CreateMap<Model.Person, PersonResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.SocialNumber, opt => opt.MapFrom(src => src.SocialNumber))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
                .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.Phones, opt => opt.MapFrom(src => GetPhones(src.Phones)));

            CreateMap<PersonResponse, Model.Person>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.SocialNumber, opt => opt.MapFrom(src => src.SocialNumber))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.Ativo))
                .ForMember(dest => dest.Phones, opt => opt.MapFrom(src => GetPhones(src.Phones)))
                .ForMember(dest => dest.CreatedAt, src => src.Ignore())
                .ForMember(dest => dest.IsDeleted, src => src.Ignore())
                .ForMember(dest => dest.UpdatedAt, src => src.Ignore())
                .ForMember(dest => dest.IsValid, src => src.Ignore())
                .ForMember(dest => dest.ValidationResult, src => src.Ignore());
        }

        private static List<UpdatePhone> GetPhones(IReadOnlyCollection<Model.Phone> phones)
        {
            var list = new List<UpdatePhone>();

            if (!phones.Any())
                return list;

            list.AddRange(phones.Select(p => new UpdatePhone()
            {
                Id = p.Id,
                Number = p.Number,
                Type = p.Type
            }));

            return list;
        }

        private static List<Model.Phone> GetPhones(IReadOnlyCollection<UpdatePhone> phones)
        {
            var list = new List<Model.Phone>();

            if (!phones.Any())
                return list;

            list.AddRange(phones.Select(p => new Model.Phone()
            {
                Id = p.Id,
                Number = p.Number,
                Type = p.Type
            }));

            return list;
        }
    }
}
