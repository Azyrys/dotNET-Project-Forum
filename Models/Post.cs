using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using dotNET_Project.Areas.Identity.Data;
namespace dotNET_Project.Models

{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Prosze podaj Tytul wiadomosci")]
        [MinLength(2), MaxLength(20)]
        public string Title { get; set; }

        [Required]
        public Topic Topic { get; set; }

        [Required(ErrorMessage = "Prosze podaj Tresc wiadomosci")]
        [MinLength(10), MaxLength(50)]
        public string Content { get; set; }
        [Required]
        public dotNET_ProjectUser User { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        [NotMapped]
        public int TopicID {  get; set; }
    }
}
