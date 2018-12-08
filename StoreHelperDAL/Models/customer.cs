using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreHelper.Dal.Core.Interfaces.Entity;

namespace StoreHelperDAL.Models
{
    public class Customer : IKeyable<int>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public virtual ICollection<Purchase> Purchases { get; set; }
    }
}
