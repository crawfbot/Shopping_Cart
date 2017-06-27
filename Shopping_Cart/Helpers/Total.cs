using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Shopping_Cart.Models;
using Microsoft.AspNet.Identity;

namespace Shopping_Cart.Helpers
{
    public static class Total
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        public static decimal calcTotal()
        {
            if (HttpContext.Current.User.Identity.GetUserId() != null)
            {
                var user = db.Users.Find(HttpContext.Current.User.Identity.GetUserId());
                var shoppingCart = db.ShoppingCarts.Where(x => x.CustomerId == user.Id);

                if (shoppingCart != null)
                {
                    var total = 0.00M;
                    foreach (var cart in shoppingCart)
                    {
                        var price = cart.Item.Price;
                        var quantity = cart.Count;
                        total += (price * quantity);
                    }

                    return total;
                }
                else
                {
                    var total = 0.00M;
                    return total;
                }

            }
            else
            {
                return 0.00M;
            }

        }
    }
}