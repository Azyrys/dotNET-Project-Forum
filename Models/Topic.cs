using System.ComponentModel.DataAnnotations;
namespace dotNET_Project.Models
{
    public class Topic
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
