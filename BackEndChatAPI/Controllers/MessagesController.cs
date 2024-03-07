using BackEndChatAPI.Models;
using BackEndChatAPI.Repos.Interface;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackEndChatAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private IMessagesRepo _messagesRepo;
        public MessagesController(IMessagesRepo messagesRepository)
        {
            _messagesRepo = messagesRepository;
        }

        // GET: api/<MessagesController>
        [HttpGet]
        public IActionResult GetAllMessages()
        {
            var messages = _messagesRepo.GetMessages();
            return Ok(messages);
        }

        // GET api/<MessagesController>/5
        [HttpGet("user")]
        public async Task <IActionResult> Get(string user)
        {
            var messages = await _messagesRepo.GetMessagesByUser(user);
            return Ok(messages);
        }

        // POST api/<MessagesController>
        [HttpPost]
        public IActionResult Post([FromBody] Messages newMessages)
        {
            var messages = _messagesRepo.addMessage(newMessages);
            return Ok(messages);
        }

        // PUT api/<MessagesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MessagesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
