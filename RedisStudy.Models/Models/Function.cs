using System.ComponentModel.DataAnnotations;

namespace RedisStudy.DAL.Abstraction.Models
{
    public class Function
    {
        [Key]
        public string Id { get; set; }

        [Display(Name = "名称")]
        public string FunName { get; set; }

        [Display(Name = "类型")]
        public string FunType { get; set; }

        [Display(Name = "说明")]
        public string FunRemark { get; set; }

        [Display(Name ="调用方式")]
        public string ExecMode { get; set; }

        [Display(Name = "是否启用")]
        public bool IsUsed { get; set; }
    }
}
