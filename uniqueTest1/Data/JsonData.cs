using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using uniqueTest1.Models;

namespace uniqueTest1.Data
{
    public class JsonData
    {//data layer
        //used in newtonsoftt librery easy and simple library for json file read/write convert to items etc.
        private Dictionary<string, string> category;
        private Order order = new Order();
        private List<Order> orders;

        public  JsonData()
            {
            // it's default options for now can edit the categories.json file and add more 
            category = new Dictionary<string, string>() { { "breakfast", "ארוחת בוקר" }, { "lunch", "ארוחת צהריים" }, { "dinner", "ארוחת ערב" } };
            orders = new List<Order>();
            // file of data
            if (File.Exists("Menu.json"))
            {
                using (StreamReader r = new StreamReader("Menu.json"))
                {
                    string json = r.ReadToEnd();
                    orders = JsonConvert.DeserializeObject<List<Models.Order>>(json);
                }
            }
            else { File.Create("Menu.json");
                   }
            // file of categories 
            if (File.Exists("Categories.json"))
            {
                using (StreamReader r = new StreamReader("Categories.json"))
                {
                    string json = r.ReadToEnd();
                    category = JsonConvert.DeserializeObject<Dictionary<string,string>>(json);
                }
            }
            else
            {
                var crtCat =  File.Create("Categories.json");
                crtCat.Close();
                List<string> r = new List<string>(); 
                var res = from i in category
                                   select (string.Concat(i.Key,":" ,i.Value)) ;
                string t = JsonConvert.SerializeObject(category);
                r.Add(t);
              /*  foreach(var item in res)
                {
                    r.Add(item);
                }*/
                File.WriteAllLines("Categories.json", r, System.Text.Encoding.UTF32);
            }
            
        }

        // return List of All Dishes
        public List<Models.Order> GetAllDishes()
        {
            if (File.Exists("Menu.json"))
            {
                using (StreamReader r = new StreamReader("Menu.json"))
                {
                    string json = r.ReadToEnd();
                    return JsonConvert.DeserializeObject<List<Models.Order>>(json);
                }
            }
            return orders;
        }

        /// <summary>
        /// get dish by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns> Order</returns>
        public Order GetDish(int id)
        {
                return orders.FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// get order with id by data without id (overload of  == !=) 
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public Order GetDish(Order order)
        {
            if (orders != null)
            {
                return orders.FirstOrDefault(x => x.Equals(order));
            }
            else
            {
                return null;
            }
            
        }


        public bool Edit(int id, Order order)
        {
                if (GetAllDishes().Exists(x=>x.Id == id))
                {
                    Delete(id);
                    orders.Add(order);
                    return PrintDish(orders);
                    
                }
            return false;

        }
        public  bool Add(Order o)
        {
            orders = new List<Order>();
            orders = GetAllDishes();
            try
            {
                if (orders != null && orders.Exists(x => x==o)) //this mean exist in list already the dish
                {
                    return false;
                }
                else
                {
                    orders.Add(o);
                    return PrintDish(orders);// update file
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            orders = new List<Order>();
            orders = GetAllDishes();
            try
            {
                orders.Remove(GetDish(id));
                return PrintDish(orders);
                 
            }
            catch
            {
                return false;
            }
        }

        public  bool PrintDish(List<Order> o)
        {
            //get Dish and put all properties and values in format like JSON inside text
            try
            {
                string res = JsonConvert.SerializeObject(o);

                List<string> str = new List<string>() { res };
                File.WriteAllLines("Menu.json", str, System.Text.Encoding.UTF32);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
           
        }
        public bool addCategory(string name)// we not used in this function right now but if want to add categories by input used in this
        {                                   // the format need still to work on it
           if(category.TryGetValue(name,out string value))
            {
                return false;
               
            }
            else
            {
                category.Add(name,value);
                List<string> r = new List<string>();
                var res = from item in category
                          select (string.Concat(item.Key, ":", item.Value));
                foreach(var item in res)
                {
                    r.Add(item);
                }
                File.WriteAllLines("Categories.json", r, System.Text.Encoding.UTF32);
                return true;
            }
        }
        public Dictionary<string,string> getCategory()//get categories from the file
        {
            if (File.Exists("Categories.json"))
            {
                using (StreamReader r = new StreamReader("Categories.json"))
                {
                    string json = r.ReadToEnd();
                    r.Close();
                    return JsonConvert.DeserializeObject<Dictionary<string,string>>(json);
                }
            }
            return null;
        }

    }
}
