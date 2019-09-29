using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreHelper.Dal.Core.Interfaces.Entity;

namespace StoreHelperDAL.Models
{
    public class Product : IKeyable<long>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public ProductType ProductType { get; set; }
        public virtual ICollection<Purchase> Purchases { get; set; }

        public override bool Equals(object obj)
        {
            var item = obj as Product;

            if (item == null)
            {
                return false;
            }

            return this.Id.Equals(item.Id);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
