using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using dotNET_Project.Areas.Identity.Data;
namespace dotNET_Project.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public Post Post { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public dotNET_ProjectUser User { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        [NotMapped] 
        public int PostID { get; set; }
    }
}
