using System.ComponentModel.DataAnnotations;

namespace ToDoWebAPI.Models
{
    public class GroupDto
    {
        public int GroupId { get; set; }

        [Required]
        public string Name { get; set; }
        
        public string UserId { get; set; }
    }
}