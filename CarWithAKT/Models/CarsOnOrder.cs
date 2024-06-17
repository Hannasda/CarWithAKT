using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarWithAKT.Models
{
    public class CarsOnOrder
    {
        public int Id { set; get; }

        public int OrderId { set; get; }
        public Order Order { set; get; }

        public int CarId { set; get; }
        public Car Car { set; get; }


        public int Count { set; get; }
    }
}