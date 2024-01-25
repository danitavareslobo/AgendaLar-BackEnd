using AgendaLarAPI.Controllers.Base;
using AgendaLarAPI.Models.Person;
using AgendaLarAPI.Services;
using AgendaLarAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgendaLarAPI.Controllers
{
    [Route("api/telefones")]
    [Authorize]
    public class PhoneController : DefaultController
    {
        private readonly IPhoneService _service;

        public PhoneController(
            IPhoneService service,
            NotificationService notificationService)
            : base(notificationService)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return CustomResponse(await _service.GetAllAsync());
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(int pageSize, int pageIndex)
        {
            return CustomResponse(await _service.GetPagedAsync(pageSize, pageIndex));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return CustomResponse(await _service.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Add(Phone phone)
        {
            return CustomResponse(await _service.AddAsync(phone));
        }

        [HttpPut]
        public async Task<IActionResult> Update(Phone phone)
        {
            return CustomResponse(await _service.UpdateAsync(phone));
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return CustomResponse(await _service.DeleteAsync(id));
        }
    }
}
