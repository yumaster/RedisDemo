using System.ComponentModel.DataAnnotations;

namespace RedisStudy.DAL.Abstraction.Models
{
    public class Function
    {
        [Key]
        public string Id { get; set; }

        [StringLength(50)]
        public string FunName { get; set; }

        [StringLength(50)]
        public string FunType { get; set; }

        [StringLength(200)]
        public string FunRemark { get; set; }


        public bool IsUsed { get; set; }
    }
}
