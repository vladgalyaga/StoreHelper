﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreHelper.Dal.Core.Interfaces.Entity;

namespace StoreHelperDAL.Models
{
    public class Purchase : IKeyable<int>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public  int Id { get; set; }



        public virtual ICollection<Product> Products { get; set; }

        public Customer Customer { get; set; }


    }
}  
