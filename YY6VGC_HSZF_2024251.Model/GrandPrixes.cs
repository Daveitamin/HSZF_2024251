using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace YY6VGC_HSZF_2024251.Model
{
    public class GrandPrixes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [JsonProperty("location")]
        public string Location { get; set; }

        [Required]
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [Required]
        [JsonProperty("podium")]
        public string[] Podium {  get; set; }
        
        [Required]
        [JsonProperty("drivers")]
        public ICollection<Drivers> Drivers { get; set;} = new List<Drivers>();





}

    public class Drivers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DriverId { get; set; }
        
        [Required]
        [JsonProperty("name")]
        public string name { get; set; }

        [Required]
        [JsonProperty("team")]
        public string team { get; set; }

        [Required]
        [JsonProperty("points")]
        public int points { get; set; }
    }
}
