using System.ComponentModel.DataAnnotations;

namespace Elmagram.Models
{
    public class NewMessageViewModel
    {
        [Required]
        public string Content { get; set; }
    }
}