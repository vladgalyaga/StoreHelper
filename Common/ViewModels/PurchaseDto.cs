using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ViewModels
{
    public class PurchaseDto
    {
        public long Id { get; set; }

        public virtual ICollection<ProductDto> Products { get; set; }
    }
}
