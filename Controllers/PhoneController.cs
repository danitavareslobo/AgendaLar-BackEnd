using AgendaLarAPI.Controllers.Base;
using AgendaLarAPI.Models.Person;
using AgendaLarAPI.Models.Person.ViewModels;
using AgendaLarAPI.Services;
using AgendaLarAPI.Services.Interfaces;
using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;

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
            return CustomResponse(_mapper.Map<List<UpdatePhone>>(await _service.GetAllAsync()));
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(int pageSize, int pageIndex)
        {
            return CustomResponse(_mapper.Map <List<UpdatePhone>>(await _service.GetPagedAsync(pageSize, pageIndex)));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return CustomResponse(await _service.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreatePhone phone)
        {
            var dto = _mapper.Map<Phone>(phone);
            var result = await _service.AddAsync(dto);
            var response = _mapper.Map<UpdatePhone>(result);
            return CustomResponse(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdatePhone phone)
        {
            var dto = _mapper.Map<Phone>(phone);
            var result = await _service.UpdateAsync(dto);
            var response = _mapper.Map<UpdatePhone>(result);
            return CustomResponse(response);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return CustomResponse(await _service.DeleteAsync(id));
        }
    }
}
