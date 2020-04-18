using System.ComponentModel.DataAnnotations;

namespace ToDoWebAPI.Core.Models
{
    public class TodoItemDto
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }
        
        public string GroupName { get; set; }

        public bool StatusId { get; set; }
    }
}