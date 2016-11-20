namespace ToDoDAL.Model
{
    public partial class ToDoList
    {
        public int NoteId { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public int GroupId { get; set; }
        public bool StatusId { get; set; }
        public string UserId { get; set; }


        public Group Group { get; set; }
    }
}