﻿using Common.ViewModels;
using StoreHelper.BLL.Contracts;
using StoreHelper.Dal.Core.Interfaces;
using StoreHelperDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreHelperBLL
{
    public class ProdustsManager : IProdustsManager
    {
        readonly IRepository<Product, long> _productRepository;
        public ProdustsManager(IUnitOfWork unitOfWork)
        {
            _productRepository = unitOfWork.GetRepository<Product, long>();
        }
        public void AddProducts(params ProductDto[] products)
        {
            var entities = products.Select(Mapper.Map).ToList();
            _productRepository.CreateRange(entities);
        }

        public void Delete(long id)
        {
            _productRepository.DeleteAsync(id);
        }

        public List<ProductDto> GetAllProtducts()
        {
            return _productRepository.GetAll().Select(Mapper.Map).ToList();
        }

        public ProductDto GetProduct(long id)
        {
            var prod = _productRepository.Find(id);
            var result = MapProduct(prod);
            return result;
        }

    }
}
