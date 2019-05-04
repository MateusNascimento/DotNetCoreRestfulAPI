using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace DotNetCoreRestfulAPI.Models
{
    public class Quote
    {
        public long Id { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public long StockId { get; set; }

        [JsonIgnore]
        public Stock Stock { get; set; }
    }
}
