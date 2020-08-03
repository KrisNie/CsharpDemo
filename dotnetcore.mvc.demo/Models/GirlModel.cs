using System.Collections.Generic;
using dotnetcore.mvc.demo.DataAccess.Entity;

namespace dotnetcore.mvc.demo.Models
{
    public class GirlModel
    {
        public List<Girl> Girls { get; set; }
        
        public List<Girl> SingleGirls { get; set; }

        public Girl YoungestOne { get; set; }

        public string PhoneNumber { get; set; }

        public string BelovedName { get; set; }
    }
}