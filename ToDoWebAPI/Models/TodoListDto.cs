using System.ComponentModel.DataAnnotations;

namespace ToDoWebAPI.Models
{
    public class TodoListDto
    {
        public int NoteId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Comment { get; set; }
        
        public string GroupName { get; set; }

        public bool StatusId { get; set; }
    }
}