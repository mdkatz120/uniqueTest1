using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using uniqueTest1.Models;
using uniqueTest1.Data;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;

namespace uniqueTest1.Controllers
{//service layer of controllers
    public class OrdersController : Controller
    {
        private JsonData jd;
        private List<Order> orders = new List<Order>();
        public static int id = 1;
        public OrdersController()
        {
            jd= new JsonData();
        }

        // GET: Orders
      
        public ActionResult Index(bool  id)
        {
           // var myTask = Task.Run(() => jd.GetAllDishes());
            if (jd.GetAllDishes() != null)
            {
                orders = jd.GetAllDishes();
                ViewData["chk"] = false; // flag for visitor allergic
                if (id)
                {
                    ViewData["chk"] = true;
                    return View(orders.FindAll(x=>x.Allergic == false));
                }
            }
            else { orders.Add(new Order()); }
            return View(orders);
        }


        // GET: Orders/Details/5
        public ActionResult Details(int id)
        {
            return View(jd.GetDish(id));
        }

        // GET: Orders/Create
       
        public ActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Admin")]//second blocked for not authorized in controller by annotation 
        public ActionResult Create(Order collection)
        {
            try
            {
                collection.Id = Math.Max(id++,jd.GetAllDishes().Count());//serial id for new item
                bool success = jd.Add(collection);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        // GET: Orders/Edit/5
        
        public ActionResult Edit(int id)
        {
            
            return View(jd.GetDish(id));
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, Order collection)
        {
            try
            {
                bool s = jd.Edit(id, collection);

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        // GET: Orders/Delete/5
        
        public ActionResult Delete(int id)
        {
            return View(jd.GetDish(id));
        }

        // POST: Orders/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id, Order collection)
        {
            try
            {
                // TODO: Add delete logic here
                jd.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        public void AddCategory(string id)
        {
            jd.addCategory(id);
        }

        /// <summary>
        /// for set categories in select
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Dictionary<string,string> getcategories()
        {
            return jd.getCategory();
        }
    }
}