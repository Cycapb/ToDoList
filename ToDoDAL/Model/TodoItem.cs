using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoDAL.Model
{
    [Table("TodoItem")]
    public class TodoItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Не задано описание")]
        [StringLength(1024)]
        public string Description { get; set; }

        public int GroupId { get; set; }

        public bool IsFinished { get; set; }

        [Required(ErrorMessage = "Не выбран пользователь")]
        [StringLength(50)]
        public string UserId { get; set; }

        [NotMapped]
        public string GroupName { get; set; }

        public virtual TodoGroup Group { get; set; }
    }
}
