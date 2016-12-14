using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Web.Mvc;

namespace ToDoDAL.Model
{
    [MetadataType(typeof(GroupMetadata))]
    public partial class Group
    {
        public static explicit operator Group(HttpContent v)
        {
            throw new NotImplementedException();
        }
    }

    public class GroupMetadata
    {
        [HiddenInput(DisplayValue = false)]
        public int GroupId { get; set; }

        [Required(ErrorMessage = "Не задано имя группы")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Не задан пользователь")]
        public string UserId { get; set; }
    }
}
