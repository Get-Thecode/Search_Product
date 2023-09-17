using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Search_Filter.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Size { get; set; }
        public decimal Price { get; set; }
        public DateTime MfgDate { get; set; }
        public string Category { get; set; }
    }
}