using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shopping_Cart.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string CustomerId { get; set; }
        public int Count { get; set; }
        public DateTimeOffset Created { get; set; }

       // public string MediaUrl { get; set; }

        public virtual Item Item { get; set; }
        public virtual ApplicationUser Customer { get; set; }

    }
}