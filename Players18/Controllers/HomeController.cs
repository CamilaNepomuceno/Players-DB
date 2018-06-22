using Players18.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;

namespace Players18.Controllers

{
    public class HomeController : Controller
    {
        private PlayersEntities1 db = new PlayersEntities1();

        public ActionResult Index(string searchString, string playerCountry)
        {
            //creating the SelectList for the Country dropdown List
            List<string> countryList = new List<string>();
            var countryQuery = from c in db.Ratings
                               orderby c.Country
                               select c.Country;
            countryList.AddRange(countryQuery.Distinct());
            ViewBag.playerCountry = new SelectList(countryList);

            // LINQ query to get all the records from the db
            //getting all the countries from the db
            var players = from p in db.Ratings
                          select p;

            //filtering by country
            if (!String.IsNullOrEmpty(playerCountry))
            {
                players = players.Where(x => x.Country == playerCountry);
            }

            //searching by Name
            if (!String.IsNullOrEmpty(searchString))
            {
                players = players.Where(x => x.Name.Contains(searchString));
            }

            //passing the data to the view
            return View(players);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Rating player)
        {
            if (ModelState.IsValid)
            {
                db.Ratings.Add(player);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(player);
        }

        public ActionResult Edit(int id)
        {
            Rating player = db.Ratings.Find(id);
            return View(player);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Rating player)
        {
            if (ModelState.IsValid)
            {
                db.Entry(player).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(player);
        }

        public ActionResult Details(int? id)
        {
            //if a null is passed in, display an HTML error page 
            //add using System.Net to get rid of the error on HttpStatusCode
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //get the record from the database using its id
            Rating player = db.Ratings.Find(id);
            //if an invalid id was passed in, and the record doesn't
            //exist in the database, display an HTML error page

            if (player == null)
            {
                return HttpNotFound();
            }
            //pass the data to the Details view to be displayed
            return View(player);
        }

        public ActionResult Delete(int id)
        {
            Rating player = db.Ratings.Find(id);
            return View(player);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Rating player = db.Ratings.Find(id);
            db.Ratings.Remove(player);
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}