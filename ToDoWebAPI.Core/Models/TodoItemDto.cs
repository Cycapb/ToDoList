using System.ComponentModel.DataAnnotations;

namespace ToDoWebAPI.Core.Models
{
    public class TodoItemDto
    {
        public int NoteId { get; set; }

        [Required]
        public string Description { get; set; }
        
        public string GroupName { get; set; }

        public bool StatusId { get; set; }
    }
}