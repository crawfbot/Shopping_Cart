using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Shopping_Cart.Models;
using Shopping_Cart.Helpers;
using Microsoft.AspNet.Identity;

namespace Shopping_Cart.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize]
        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var count = db.ShoppingCarts.Count();
            if (count == 0 && user.Id != null)
            {
                ViewBag.Message = 0;
            }
            else
            {

                ViewBag.num = db.ShoppingCarts.Where(x => x.CustomerId == user.Id).Sum(m => m.Item.Price * m.Count);
                ViewBag.total = ViewBag.num;
            }

            //return View(db.ShoppingCarts.ToList());

            var shoppingCarts = db.ShoppingCarts.Include(s => s.Customer);
            return View(shoppingCarts.ToList());
        }

        // GET: ShoppingCarts --- OLD CODE
        //Added authorize

        //[Authorize]
        //public ActionResult Index()
        //{

        //    //User.Identity.GetUserId() returns the Primary Key of the logged in User from the aSPNetUsers (Users) Table
        //    var myId = User.Identity.GetUserId();

        //    //Using the Id I will go out to the Users table and grab the entire record
        //    var user = db.Users.Find(myId);


        //    var shoppingCarts = db.ShoppingCarts.Where(s => s.CustomerId == user.Id).ToList();
        //    if (shoppingCarts != null)
        //    {
        //        return View(shoppingCarts);
        //    }
        //    ViewBag.NoItem = "No item has been added";
        //    return View();
        //}

        // GET: ShoppingCarts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingCart shoppingCart = db.ShoppingCarts.Find(id);
            if (shoppingCart == null)
            {
                return HttpNotFound();
            }
            return View(shoppingCart);
        }

       
        // GET: ShoppingCarts/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.ItemId = new SelectList(db.Items, "Id", "Name");
            return View();
        }

        // POST: ShoppingCarts/Create

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int Itemid)
        {
            //This code presupposes that the user is logged in. User.Identity.GetUserId acts upon a logged in user...
            var user = db.Users.Find(User.Identity.GetUserId());

            //If the user is not logged in this line of code will throw an exception because user.Id is null
            var exShopping = db.ShoppingCarts.Where(s => s.CustomerId == user.Id && s.ItemId == Itemid).ToList();

            //If and only if the Users cart does not already have this particular item we will execute this code 
            if (exShopping.Count == 0)
            {
                ShoppingCart shoppingCart = new ShoppingCart();
                shoppingCart.CustomerId = user.Id;
                shoppingCart.ItemId = Itemid;
                shoppingCart.Item = db.Items.FirstOrDefault(i => i.Id == Itemid);
                shoppingCart.Count = 1;
                shoppingCart.Created = System.DateTime.Now;
                db.ShoppingCarts.Add(shoppingCart);
                db.SaveChanges();
                return RedirectToAction("Index", "ShoppingCarts");
            }

            //Otherwise we will execute this code block which will simply increment the item count
            foreach (var items in exShopping)
            {
                items.Count++;
                db.Entry(items).Property("Count").IsModified = true;
            };

            db.SaveChanges();

            return RedirectToAction("Index", "ShoppingCarts");
        }

        //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^CODE FROM CLASS ^^^^^^^^^^^^

        //Commenting out THE CODE BELOW to add code from class

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,ItemId,CustomerId,Count,Created")] ShoppingCart shoppingCart)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.ShoppingCarts.Add(shoppingCart);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.CustomerId = new SelectList(db.Users, "Id", "FirstName", shoppingCart.CustomerId);
        //    ViewBag.ItemId = new SelectList(db.Items, "Id", "Name", shoppingCart.ItemId);
        //    return View(shoppingCart);
        //}





        // GET: ShoppingCarts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingCart shoppingCart = db.ShoppingCarts.Find(id);
            if (shoppingCart == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Users, "Id", "FirstName", shoppingCart.CustomerId);
            ViewBag.ItemId = new SelectList(db.Items, "Id", "Name", shoppingCart.ItemId);
            return View(shoppingCart);
        }

        // POST: ShoppingCarts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ItemId,CustomerId,Count,Created")] ShoppingCart shoppingCart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shoppingCart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Users, "Id", "FirstName", shoppingCart.CustomerId);
            ViewBag.ItemId = new SelectList(db.Items, "Id", "Name", shoppingCart.ItemId);
            return View(shoppingCart);
        }

        // GET: ShoppingCarts/Delete/5

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingCart shoppingCart = db.ShoppingCarts.Find(id);
            if (shoppingCart == null)
            {
                return HttpNotFound();
            }
            return View(shoppingCart);
        }

        // POST: ShoppingCarts/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ShoppingCart shoppingCart = db.ShoppingCarts.Find(id);
            db.ShoppingCarts.Remove(shoppingCart);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
