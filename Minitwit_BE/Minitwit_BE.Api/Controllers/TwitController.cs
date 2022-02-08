using Microsoft.AspNetCore.Mvc;
using Minitwit_BE.Domain;
using Minitwit_BE.Persistence;

namespace Minitwit_BE.Api.Controllers
{
    [ApiController]
    [Route("api/twit")]
    public class TwitController : ControllerBase
    {
        [HttpGet("test")]
        public async Task<string> TestEndpoint()
        {
            return await Task.FromResult("test");
        }

        // TODO: Adding to the database should be POST. The message should be taken from the request body.
        [HttpGet("add")]
        public async Task<string> AddTwit()
        {
            using (var db = new TwitContext())
            {
                // Add new twit
                Console.WriteLine("Inserting a new twit");
                // TODO: It has to be investigated how to autoincrement primary keys, etc.
                db.Add(new Message 
                { 
                    AuthorId = 1,
                    Flagged = false,
                    PublishDate = DateTime.Now,
                    MessageId = 1,
                    Text = "TestText"
                });
                db.SaveChanges();
            }

            return "OK";
        }

        [HttpGet("getall")]
        public async Task<string> GetTwit()
        {
            using (var db = new TwitContext())
            {
                // Print all to console
                Console.WriteLine("Reading");
                db.Messages.OrderBy(m => m.MessageId).AsEnumerable().ToList().ForEach(e => Console.WriteLine(e.Text));
            }

            return "OK";
        }
    }
}
