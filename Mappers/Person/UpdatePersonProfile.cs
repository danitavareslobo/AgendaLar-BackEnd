using AgendaLarAPI.Models.People.ViewModels;

using AutoMapper;

using Model = AgendaLarAPI.Models.People;

namespace AgendaLarAPI.Mappers.Person
{
    public class UpdatePersonProfile : Profile
    {
        public UpdatePersonProfile()
        {
            CreateMap<UpdatePerson, Model.Person>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id)))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.SocialNumber, opt => opt.MapFrom(src => src.SocialNumber))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.Ativo))
                .ForMember(dest => dest.Phones, opt => opt.MapFrom(src => AddPhones(src)));
        }

        private static List<Model.Phone> AddPhones(UpdatePerson dto)
        {
            var list = new List<Model.Phone>();

            if (dto.UpdatedPhones.Count > 0)
            {
                list.AddRange(dto.UpdatedPhones.Select(u => new Model.Phone
                {
                    Id = Guid.Parse(u.Id),
                    PersonId = Guid.Parse(dto.Id!),
                    Type = u.Type,
                    Number = u.Number
                }));
            }
            
            if(dto.NewPhones.Count > 0)
            {
                list.AddRange(dto.NewPhones.Select(n => new Model.Phone
                {
                    PersonId = Guid.Parse(dto.Id!),
                    Type = n.Type,
                    Number = n.Number
                }));
            }

            var uniqueNumbers = list.Distinct(new PhoneEqualityComparer()).ToList();

            return uniqueNumbers;
        }
    }

    public class PhoneEqualityComparer : IEqualityComparer<Model.Phone>
    {
        public bool Equals(Model.Phone x, Model.Phone y)
        {
            return x.Number == y.Number;
        }

        public int GetHashCode(Model.Phone obj)
        {
            return obj.Number.GetHashCode();
        }
    }
}
