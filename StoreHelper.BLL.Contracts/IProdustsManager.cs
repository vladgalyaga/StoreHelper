using Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreHelper.BLL.Contracts
{
    public interface IProdustsManager
    {
        void AddProducts(params ProductDto[] products);

        List<ProductDto> GetAllProtducts();

        ProductDto GetProduct(long id);

        void Delete(long id);
    }
}
