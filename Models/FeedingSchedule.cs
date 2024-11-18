using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WlidLife.Models
{
    public class FeedingSchedule
    {
        [Key]
        public int FeedingId { get; set; }

        [Required]
        public DateTime? FeedingTimes { get; set; }

        public string? DietRequirements { get; set; }

        
        public int AnimalId { get; set; }

       
        

        
        public int CaretakerId { get; set; }

        [JsonIgnore]
        public Animals? Animals { get; set; }

        [JsonIgnore]
       
        public Caretaker? Caretaker { get; set; }
    }
}
