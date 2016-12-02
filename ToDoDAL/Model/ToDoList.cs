namespace ToDoDAL.Model
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ToDoList")]
    public partial class ToDoList
    {
        [Key]
        public int NoteId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Comment { get; set; }

        public int GroupId { get; set; }

        public bool StatusId { get; set; }

        [Required]
        public string UserId { get; set; }        

        public virtual Group Group { get; set; }
    }
}
