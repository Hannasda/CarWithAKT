using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarWithAKT.ViewModels
{
    public class ViewModelForBasket
    {

        public int Id { set; get; }
        public string Name { set; get; }
        public int Price { set; get; }
        public string Modelname { set; get; }
        public string Img { set; get; }

        public int Count { set; get; }
    }
}