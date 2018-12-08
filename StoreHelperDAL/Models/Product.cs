using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreHelper.Dal.Core.Interfaces.Entity;

namespace StoreHelperDAL.Models
{
    public class Product : IKeyable<int>
    {
        public  int Id { get; set; }
        public  string Name { get; set; }
        public  double Price { get; set; }

        public ProductType ProductType { get; set; }
        public ICollection<Purchase> Purchases { get; set; }
    }
}
