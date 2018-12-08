using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreHelper.BLL.Contracts;
using StoreHelper.Dal.Core.Interfaces;

namespace StoreHelperBLL
{
    public class PurchaseManager : IPurchaseManager
    {
        private IUnitOfWork _unitOfWork;

        public PurchaseManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

    }
}
