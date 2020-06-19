using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcjQueryTest.Models
{
    public class jobdetailsDTO
    {
        public int JobID { get; set; }
        public string Task_Name { get; set; }
        public string Description { get; set; }
        public string Date_Started { get; set; }
        public string Date_Finished { get; set; }
        public string Status { get; set; }
    }
}