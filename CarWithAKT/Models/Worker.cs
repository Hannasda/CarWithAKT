using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarWithAKT.Models
{
    public class Worker
    {
        public int Id { set; get; }
        public int? ClientId { set; get; }
        public Client Client { set; get; }
        public string Middlename { set; get; }
        public DateTime Start { set; get; }
    }
}