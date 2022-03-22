using Minitwit_BE.Domain;

namespace Minitwit_BE.Api.Dtos.FE
{
    public class TweetDto
    {

        public List<Message>? tweets { get; set; }

        public int? page { get; set; }

        public int? totalPages { get; set; }

    }
}
