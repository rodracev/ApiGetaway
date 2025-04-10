using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGetaway.Models
{
    public class Apikey
    {
        public int Id { get; set; }
        public string Key { get; set; } = string.Empty;
        public string Owner { get; set; } = string.Empty;
        public string Role { get; set; } = "basic"; // Por ejemplo: admin, read-only, etc.
    }
}