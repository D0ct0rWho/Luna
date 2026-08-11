using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Luna.Dtos;
using Luna.Services;
using System.Security.Claims;

namespace Luna.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactsController(IContactService contactService)
        {
            _contactService = contactService;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactDto>>> GetContacts()
        {
            var contacts = await _contactService.GetContactsAsync(GetUserId());
            return Ok(contacts);
        }

        [HttpPost]
        public async Task<ActionResult<ContactDto>> AddContact(CreateContactRequestDto request)
        {
            var contact = await _contactService.AddContactAsync(GetUserId(), request);
            return CreatedAtAction(nameof(GetContacts), new { id = contact.Id }, contact);
        }

        [HttpPut("{id}/accept")]
        public async Task<IActionResult> AcceptContact(int id)
        {
            await _contactService.AcceptContactAsync(GetUserId(), id);
            return NoContent();
        }

        [HttpPut("{id}/reject")]
        public async Task<IActionResult> RejectContact(int id)
        {
            await _contactService.RejectContactAsync(GetUserId(), id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveContact(int id)
        {
            await _contactService.RemoveContactAsync(GetUserId(), id);
            return NoContent();
        }
    }
}