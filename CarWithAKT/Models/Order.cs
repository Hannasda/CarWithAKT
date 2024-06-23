using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarWithAKT.Models
{
    public class Order
    {
        public int Id { set; get; }

        public int? WorkerId { set; get; }
        public Worker Worker { set; get; }

        public int ClientId { set; get; }
        public Client Client { set; get; }

        public int StatusId { set; get; }
        public Status Status { set; get; }

        public int LocationId { set; get; }
        public Location Location { set; get; }

        public int FullPrice { set; get; }

        [Required(ErrorMessage = "Введіть бажану дату для передачі авто")]
        [DataType(DataType.Date)]
        public DateTime Date { set; get; }
        public DateTime CreateDate { set; get; }
    }
}