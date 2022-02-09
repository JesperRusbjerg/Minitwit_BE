using Microsoft.AspNetCore.Mvc;
using Minitwit_BE.Domain;
using Minitwit_BE.Persistence;

namespace Minitwit_BE.Api.Controllers
{
    [ApiController]
    [Route("api/twit")]
    public class TwitController : ControllerBase
    {
        private readonly TwitContext _twitContext;          // To dependency inject the context instance.
        
        public TwitController(TwitContext twitContext)
        {
            _twitContext = twitContext;
        }
        
        [HttpGet("test")]
        public async Task<string> TestEndpoint()
        {
            return await Task.FromResult("test");
        }

        // TODO: Adding to the database should be POST/PUT. The message should be taken from the request body.
        [HttpGet("add")]
        public async Task<string> AddTwit()
        {
            
            // Add new twit
            Console.WriteLine("Inserting a new twit");
            // Primary keys should be auto incremented when you add entity to the table and dont explicitely specify specify the ID
            _twitContext.Add(new Message 
            { 
                AuthorId = 1,
                Flagged = false,
                PublishDate = DateTime.Now,
                Text = "jjjjjjjjjjjjjjj"
            });
            _twitContext.SaveChanges();
            

            return "OK";
        }

        [HttpGet("getall")]
        public async Task<string> GetTwit()
        {
            // Print all to console
            Console.WriteLine("Reading");
            _twitContext.Messages.OrderBy(m => m.MessageId).AsEnumerable().ToList().ForEach(e => Console.WriteLine($"Id: {e.MessageId}, Text: {e.Text}"));

            return "OK";
        }
    }
}
