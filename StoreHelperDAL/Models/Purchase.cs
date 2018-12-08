using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreHelper.Dal.Core.Interfaces.Entity;

namespace StoreHelperDAL.Models
{
    public class Purchase : IKeyable<int>
    {
        public  int Id { get; set; }

        public virtual IEnumerable<ProductType> Productes { get; set; }
        
    }
}  
