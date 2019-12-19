using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace URLShortener.Models
{
    public class URLShort
    {
        public int Id { get; set; }
        public string LongURL { get; set; }
        public string ShortURL { get; set; }
        public string UserId { get; set; }
    }
}
