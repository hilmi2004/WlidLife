using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WlidLife.Models
{
    public class Caretaker
    {
        [Key]
        public int Id { get; set; }

        
        public string Name { get; set; }

       
        public string Specialization { get; set; }

        
        public string AssignedAnimals { get; set; }

        [JsonIgnore]
        public List<Animals>? Animals { get; set; }
    }
}
