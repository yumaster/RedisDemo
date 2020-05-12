using System.ComponentModel.DataAnnotations;

namespace RedisStudy.DAL.Abstraction.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        [StringLength(50)]
        public string Password { get; set; }
    }
}
