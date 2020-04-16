using System.ComponentModel.DataAnnotations;

namespace ToDoWebAPI.Core.Models
{
    public class TodoGroupDto
    {
        public int GroupId { get; set; }

        [Required]
        public string Name { get; set; }
        
        public string UserId { get; set; }
    }
}