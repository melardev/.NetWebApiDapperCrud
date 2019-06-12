using System;
using System.ComponentModel.DataAnnotations;

namespace WebApiDapperCrud.Entities
{
    public class Todo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title should be set")]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Completed flag should be set")]
        public bool Completed { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
    }
}