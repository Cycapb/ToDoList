namespace ToDoWebAPI.Models
{
    public class TodoListDto
    {
        public int NoteId { get; set; }

        public string Name { get; set; }

        public string Comment { get; set; }

        public int GroupId { get; set; }
        public string GroupName { get; set; }

        public bool StatusId { get; set; }
        
        public string UserId { get; set; }
    }
}