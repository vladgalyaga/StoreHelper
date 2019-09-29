using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.ViewModels;
using StoreHelperDAL.Models;

namespace StoreHelper.BLL.Contracts
{
    public interface IPurchaseManager
    {
        ProductDto MakePurchase(List<long> productIds);
    }
}
