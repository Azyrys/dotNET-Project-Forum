using System.ComponentModel.DataAnnotations;
namespace dotNET_Project.Models
{
    public class Topic
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Prosze podaj Tytul wiadomosci")]
        [MinLength(2), MaxLength(15)]
        public string Name { get; set; }
    }
}
