using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QL_BanDoAn.Models;

namespace QL_BanDoAn.Controllers
{
    public class LocationController : Controller
    {
        IFirebaseConfig config = new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = "zBat9CHGhKLYcr9BcldeMq4i0dS2rMxFhd3w36h3",
            BasePath = "https://qlbandoan-6f252-default-rtdb.asia-southeast1.firebasedatabase.app/"
        };
        IFirebaseClient client;
        // GET: Location
        public ActionResult Index()
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Location");
            var data = JsonConvert.DeserializeObject<List<Location>>(response.Body);
            var list = new List<Location>();

            foreach (var item in data)
            {
                var loc = JsonConvert.DeserializeObject<Location>(JsonConvert.SerializeObject(item));
                list.Add(loc);
            }

            return View(list);
        }

        public ActionResult Edit(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            var response = client.Get("Location/" + id);
            var loc = response.ResultAs<Location>();

            return View(loc);
        }

        [HttpPost]
        public ActionResult Edit(int id, string Loca)
        {
            Location loc = new Location
            {
                Id = id,
                Loc = Loca,
            };

            client = new FireSharp.FirebaseClient(config);
            client.Set("Location/" + id, loc);
            return RedirectToAction("Index");
        }
    }
}