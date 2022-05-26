using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoDomainModels.Model
{
    [Table("TodoGroup")]
    public class TodoGroup
    {
        public TodoGroup()
        {
            TodoItems = new HashSet<TodoItem>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Не задано имя группы")]
        [StringLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Не задан пользователь")]
        [StringLength(50)]
        public string UserId { get; set; }
                
        public virtual ICollection<TodoItem> TodoItems { get; set; }
    }
}
