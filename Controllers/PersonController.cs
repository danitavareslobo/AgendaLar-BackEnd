using AgendaLarAPI.Controllers.Base;
using AgendaLarAPI.Models.Person;
using AgendaLarAPI.Models.Person.ViewModels;
using AgendaLarAPI.Services;
using AgendaLarAPI.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgendaLarAPI.Controllers
{
    [Route("api/pessoas")]
    [Authorize]
    public class PersonController : DefaultController
    {
        private readonly IPersonService _service;
        private readonly IMapper _mapper;

        public PersonController(
            IPersonService service,
            IMapper mapper,
            NotificationService notificationService)
            : base(notificationService)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return CustomResponse(_mapper.Map<List<PersonResponse>>(await _service.GetAllAsync()));
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(int pageSize, int pageIndex)
        {
            return CustomResponse(await _service.GetPagedAsync(pageSize, pageIndex));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return CustomResponse(_mapper.Map<PersonResponse>(await _service.GetByIdAsync(id)));
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreatePerson person)
        {
            return CustomResponse(_mapper.Map<List<PersonResponse>>(await _service.AddAsync(_mapper.Map<Person>(person))));
        }

        [HttpPut]
        public async Task<IActionResult> Update(Person person)
        {
            return CustomResponse(await _service.UpdateAsync(_mapper.Map<Person>(person)));
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return CustomResponse(await _service.DeleteAsync(id));
        }
    }
}