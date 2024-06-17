using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarWithAKT.Models
{
    public class Basket
    {
        public int Id { set; get; }

        public int ClientId { set; get; }
        public Client Client { set; get; }

        public int CarId { set; get; }
        public Car Car { set; get; }


        public int Count { set; get; }
        public Basket() { Count = 1; }
    }
}