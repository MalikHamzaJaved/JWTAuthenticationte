using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShopOnClick.Models
{
    public class Token
    {
       
        public string token { get; set; }
        public string expiration { get; set; }
    }
}
