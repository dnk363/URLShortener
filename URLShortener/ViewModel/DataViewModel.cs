using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using URLShortener.Models;

namespace URLShortener.ViewModel
{
    public class DataViewModel
    {
        public IEnumerable<URLShort> UrlData { get; set; }
    }
}
