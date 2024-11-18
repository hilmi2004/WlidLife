using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WlidLife.Models
{
    public class Animals
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Habitat { get; set; }

        public DateTime DOB { get; set; }

        public bool EndangeredStatus { get; set; }
        
        [ForeignKey("Caretaker")]
        public int CaretakerId { get; set; }

        



    }
}
