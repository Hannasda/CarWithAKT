using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarWithAKT.Models
{
    public class Car
    {
        public int Id { set; get; }
        public string Color { set; get; }
        public string Body_type { set; get; }
        public string Fuel_type { set; get; }
        public int Fuel_use { set; get; }
        public int Engine_cap { set; get; }
        public int Mass { set; get; }
        public string Engine_type { set; get; }
        public int Seats_num { set; get; }
        public int Door_num { set; get; }
        public int Power { set; get; }
        public string Driver_type { set; get; }
        public string Transmission_type { set; get; }
        public int Price { set; get; }
        public string Modelname { set; get; }
        public string Img { set; get; }

        public int FirmId { set; get; }
        public Firm Firm { set; get; }

        public int CountryId { set; get; }
        public Country Country { set; get; }
    }
}