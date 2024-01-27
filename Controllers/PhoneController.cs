using AgendaLarAPI.Controllers.Base;
using AgendaLarAPI.Models.People;
using AgendaLarAPI.Models.People.ViewModels;
using AgendaLarAPI.Services;
using AgendaLarAPI.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgendaLarAPI.Controllers
{
    [Route("api/telefones")]
    [Authorize]
    public class PhoneController : DefaultController
    {
        private readonly IPhoneService _service;
        private readonly IMapper _mapper;

        public PhoneController(
            IPhoneService service,
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
            return CustomResponse(_mapper.Map<List<UpdatePhone>>(await _service.GetAllAsync(LoggedUserId)));
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(int pageSize, int pageIndex)
        {
            return CustomResponse(_mapper.Map<List<UpdatePhone>>(await _service.GetPagedAsync(LoggedUserId, pageSize, pageIndex)));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return CustomResponse(_mapper.Map<UpdatePhone>(await _service.GetByIdAsync(LoggedUserId, id)));
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreatePhone phone)
        {
            var entity = _mapper.Map<Phone>(phone);
            entity.UserId = LoggedUserId;
            var response = _mapper.Map<UpdatePhone>(await _service.AddAsync(entity));
            return CustomResponse(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdatePhone phone)
        {
            var entity = _mapper.Map<Phone>(phone);
            entity.UserId = LoggedUserId;
            var response = _mapper.Map<UpdatePhone>(await _service.UpdateAsync(entity));
            return CustomResponse(response);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return CustomResponse(await _service.DeleteAsync(LoggedUserId, id));
        }
    }

}
