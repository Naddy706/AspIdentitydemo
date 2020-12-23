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
using System.Data.SqlClient;
using System.Configuration;

namespace AspIdentitydemo.Controllers
{


    //[Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
       
        //public int[] a;

        // GET: Products
        public ActionResult Index()
        {


            //string[] values;
            //List<string> ci = new List<string>();
            //int c = 0;

            //var con = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            //System.Data.SqlClient.SqlConnection myConnection = new System.Data.SqlClient.SqlConnection(con);

            //System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand("select Category_Id from Products", myConnection);

            //myConnection.Open();
            //System.Data.SqlClient.SqlDataReader Reader = command.ExecuteReader();
            //while (Reader.Read())
            //{
            //    ci.Add(Reader["Category_Id"].ToString());

            //}
            //myConnection.Close();

            //foreach (var i in ci)
            //{

            //    values = i.Split(',');
            //    for (int j = 0; j < values.Length; j++)
            //    {
            //        values[j] = values[j].Trim();
            //        System.Diagnostics.Debug.WriteLine("catid" + values[j]);
            //        System.Data.SqlClient.SqlCommand command1 = new System.Data.SqlClient.SqlCommand("select Name from Catagories where id=" + values[j], myConnection);

            //        myConnection.Open();
            //        System.Data.SqlClient.SqlDataReader Reader1 = command1.ExecuteReader();
            //        while (Reader1.Read())
            //        {

            //            Reader1["Name"].ToString();
            //        }
            //        myConnection.Close();
            //    }

            //}

            List<Product> products = db.Products.ToList();
            List<Brand> Brands = db.Brands.ToList();
            List<Catagory> Categories = db.Catagories.ToList();
          


            var data=(from p in products
             join b in Brands on p.Brand_Id equals b.Id into table1
             from b in table1.DefaultIfEmpty()
             //join c in Categories on p.Category_Id equals c.Id into table2
             //from c in table2.DefaultIfEmpty()
             select new showProducts { Products = p, Brands = b ,  });


            return View(data.ToList());
        }


        public JsonResult getcat(int id)
        {
            var cat = db.Catagories.Where(x=>x.Id==id).Select(x=>x.Name);
            return Json(cat, JsonRequestBehavior.AllowGet);
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
                    ViewBag.Categories = new SelectList(db.Catagories.ToList(), "Id", "Name");

                    ViewBag.Brands = new SelectList(db.Brands.ToList(), "Id", "Name");
                    product.Category_Id = string.Join(",", product.Category_Ids);
                    System.Diagnostics.Debug.WriteLine("Form data :" + product.Brand_Id, " cat id " + product.Category_Id);
                    
                    db.Products.Add(product);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }


            }
            System.Diagnostics.Debug.WriteLine("Form data :" + product.Brand_Id," cat id "+ product.Category_Id);
            ViewBag.Categories = new SelectList(db.Catagories.ToList(), "Id", "Name");

            ViewBag.Brands = new SelectList(db.Brands.ToList(), "Id", "Name");
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
