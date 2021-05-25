using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace uniqueTest1.Models
{
    public class Order
    {

            public int Id { get; set; }
            [Display(Name = "שם המנה")]
            public string DishName { get; set; }
            [Display(Name = "מחיר")]
            public double Price { get; set; }
            [Display(Name = "חומרים אלרגניים")]
            public bool Allergic { get; set; }
            [Display(Name = "קטגוריה")]
            public string TypeDish { get; set; }
        
        //overloading operators for to have option to compare between two Dishes
        public static bool operator ==(Order a,Order b)
        {
           
           return (a.Price == b.Price && a.TypeDish == b.TypeDish && a.DishName == b.DishName && a.Allergic == b.Allergic? true: false);
        }
        public static bool operator !=(Order a,Order b)
        {
            return (a.Price != b.Price || a.TypeDish != b.TypeDish || a.DishName != b.DishName || a.Allergic != b.Allergic ? true : false);
        }

        public override bool Equals(object obj)
        {       
            // If the passed object is null
            if (obj == null)
            {
                return false;
            }
            if (!(obj is Order))
            {
                return false;
            }
            return this == (Order)obj;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}
