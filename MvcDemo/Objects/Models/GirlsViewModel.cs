using System.Collections.Generic;
using MvcDemo.Data.Girl;

namespace MvcDemo.Object.Models
{
    public class GirlsViewModel
    {
        public List<GirlEntity> Girls { get; set; }
        
        public List<GirlEntity> SingleGirls { get; set; }

        public GirlEntity YoungestOne { get; set; }

        public string PhoneNumber { get; set; }

        public string BelovedName { get; set; }
    }
}