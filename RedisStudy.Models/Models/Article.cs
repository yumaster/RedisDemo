using System.ComponentModel.DataAnnotations;

namespace RedisStudy.DAL.Abstraction.Models
{
    public class Article
    {
        [Key]
        public string Id { get; set; }

        public string UserId { get; set; }

        [StringLength(30)]
        public string Title { get; set; }

        [StringLength(50)]
        public string Content { get; set; }
    }
}
