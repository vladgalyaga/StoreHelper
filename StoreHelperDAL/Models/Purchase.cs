using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreHelper.Dal.Core.Interfaces.Entity;

namespace StoreHelperDAL.Models
{
    public class Purchase : IKeyable<long>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }



        public virtual ICollection<Product> Products { get; set; }

        public Customer Customer { get; set; }


    }
}  
