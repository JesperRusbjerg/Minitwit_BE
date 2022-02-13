using System.ComponentModel.DataAnnotations;

namespace Minitwit_BE.Domain
{
    public class Follower
    {
        [Key]
        public int Id { get; set; }
        public int WhoId { get; set; }
        public int WhomId { get; set; }
    }
}
