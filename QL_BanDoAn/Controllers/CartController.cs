using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using QL_BanDoAn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QL_BanDoAn.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        IFirebaseConfig config = new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = "zBat9CHGhKLYcr9BcldeMq4i0dS2rMxFhd3w36h3",
            BasePath = "https://qlbandoan-6f252-default-rtdb.asia-southeast1.firebasedatabase.app/"
        };
        IFirebaseClient client;
        // GET: Cart
        public ActionResult Index()
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Cart");
            var data = JsonConvert.DeserializeObject<List<Cart>>(response.Body);
            var list = new List<Cart>();

            foreach (var item in data)
            {
                var cart = JsonConvert.DeserializeObject<Cart>(JsonConvert.SerializeObject(item));
                list.Add(cart);
            }

            ViewBag.CusName = getNameCustomer(list);

            return View(list);
        }

        private List<string> getNameCustomer(List<Cart> carts)
        {
            List<String> lstStr = new List<string>();
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Customer");
            var data = JsonConvert.DeserializeObject<List<Customer>>(response.Body);
            var list = new List<Customer>();

            foreach (var item in data)
            {
                var cus = JsonConvert.DeserializeObject<Customer>(JsonConvert.SerializeObject(item));
                list.Add(cus);
            }

            foreach (var cart in carts)
            {
                foreach (var item in list)
                {
                    if (item.email == cart.email)
                        lstStr.Add(item.name);
                }
            }

            return lstStr;
        }

        [HttpGet]
        public ActionResult Detail(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Cart/" + id);
            Cart data = JsonConvert.DeserializeObject<Cart>(response.Body);

            ViewBag.Cus = GetCustomer(data.email);

            return View(data);
        }

        private Customer GetCustomer(string email)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Customer");
            var data = JsonConvert.DeserializeObject<List<Customer>>(response.Body);
            var list = new List<Customer>();

            foreach (var item in data)
            {
                var cus = JsonConvert.DeserializeObject<Customer>(JsonConvert.SerializeObject(item));
                list.Add(cus);
            }

            foreach(var cus in list)
            {
                if (cus.email == email)
                    return cus;
            }

            return null;
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Cart/" + id);
            Cart data = response.ResultAs<Cart>();
            data.status = true;

            client = new FireSharp.FirebaseClient(config);
            client.Set("Cart/" + data.id, data);

            return Redirect("/Cart/Detail/"+id);
        }
    }
}