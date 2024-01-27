using AgendaLarAPI.Controllers.Base;
using AgendaLarAPI.Models;
using AgendaLarAPI.Models.People;
using AgendaLarAPI.Models.People.ViewModels;
using AgendaLarAPI.Services;
using AgendaLarAPI.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgendaLarAPI.Controllers
{
    [Route("api/pessoas")]
    [Authorize(Roles = AgendaConstants.AdminRole)]
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
            return CustomResponse(_mapper.Map<List<PersonResponse>>(await _service.GetAllAsync(LoggedUserId)));
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(int pageSize, int pageIndex)
        {
            return CustomResponse(_mapper.Map<List<PersonResponse>>(await _service.GetPagedAsync(LoggedUserId, pageSize, pageIndex)));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return CustomResponse(_mapper.Map<PersonResponse>(await _service.GetByIdAsync(LoggedUserId, id)));
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreatePerson person)
        {
            var entity = _mapper.Map<Person>(person);
            entity.UserId = LoggedUserId;
            return CustomResponse(_mapper.Map<PersonResponse>(await _service.AddAsync(entity)));
        }

        [HttpPut]
        public async Task<IActionResult> Update(Person person)
        {
            var entity = _mapper.Map<Person>(person);
            entity.UserId = LoggedUserId;
            return CustomResponse(_mapper.Map<PersonResponse>(await _service.UpdateAsync(entity)));
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return CustomResponse(await _service.DeleteAsync(LoggedUserId, id));
        }
    }
}