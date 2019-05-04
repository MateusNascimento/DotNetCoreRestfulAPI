using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DotNetCoreRestfulAPI.Models
{
    public class Stock
    {
        public long Id { get; set; }

        [Required]
        public string Symbol { get; set; }

        [Required]
        public string Company { get; set; }

        [JsonIgnore]
        public List<Quote> Quotes { get; set; }
    }
}
