﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace ToDoDAL.Model
{
    [MetadataType(typeof(ToDoListMetadata))]
    public partial class ToDoList
    {
        [NotMapped]
        public string GroupName { get; set; }
    }

    public class ToDoListMetadata
    {
        [HiddenInput(DisplayValue = false)]
        public int NoteId { get; set; }
        [Required(ErrorMessage = "Не задано название")]
        public string Name { get; set; }
        public string Comment { get; set; }
        [Required(ErrorMessage = "Не задана группа")]
        public int GroupId { get; set; }
        [Required(ErrorMessage = "Не установлен статус")]
        public bool StatusId { get; set; }
        [Required(ErrorMessage = "Не выбран пользователь")]
        public string UserId { get; set; }
    }
}
