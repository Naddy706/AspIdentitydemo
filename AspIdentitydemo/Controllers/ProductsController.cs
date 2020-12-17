using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using AspIdentitydemo.Models;
using System.IO;

namespace AspIdentitydemo.Controllers
{


    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Products
        public ActionResult Index()
        {
            //List<string> list = new List<string>();
            //foreach (var image in db.Products.ToList()) {
            //    var d =image.ImagePath.Split(',');
            //    list.Add(d.ToString());
            //}
            //for (var i = 0; i < list.Count(); i++)
            //{

            //ViewData["images"] += list[i];
                
            //}
          //  System.Diagnostics.Debug.WriteLine("imagePath : " + list);
            // now you have the same as in the first line
            return View(db.Products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.Categories = new SelectList(db.Catagories.ToList(), "Id", "Name");

            ViewBag.Brands = new SelectList(db.Brands.ToList(), "Id", "Name");

            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                List<string> list = new List<string>();

                foreach (HttpPostedFileBase postedFileBase in product.ImageFiles)
                {

                    string FileNama = Path.GetFileNameWithoutExtension(postedFileBase.FileName);
                    string extension = Path.GetExtension(postedFileBase.FileName);
                    FileNama = FileNama + DateTime.Now.ToString("yymmssffff") + extension;

                    list.Add("~/Image/" + FileNama);

                    //   string[] str = list.ToArray();


                    FileNama = Path.Combine(Server.MapPath("~/Image/"), FileNama);
                    postedFileBase.SaveAs(FileNama);


                }

                for (var i = 0; i < list.Count(); i++)
                {

                    product.ImagePath += list[i] + ",";
                }

                if (product.ImagePath.Length < 1)
                {

                    ViewBag.message = "Image can not be empty";
                }
                else
                {

                    System.Diagnostics.Debug.WriteLine("imagePath" + product.ImagePath);

                    db.Products.Add(product);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }


            }
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Price,ImagePath,Category_Id,Brand_Id")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
