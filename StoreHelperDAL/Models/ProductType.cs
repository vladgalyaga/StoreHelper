using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreHelper.Dal.Core.Interfaces.Entity;

namespace StoreHelperDAL.Models
{
    public class ProductType : IKeyable<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual  ICollection<Product> Products { get; set; }
    }
}
