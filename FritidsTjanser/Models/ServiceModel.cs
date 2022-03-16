using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FritidsTjanser.Models
{
    //Model that matches the Service table in the database
    public class ServiceModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public int ZipCode { get; set; }
    }
}