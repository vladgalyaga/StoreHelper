using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreHelperDAL
{
    public class Configuration : DbMigrationsConfiguration<StoreHelperContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;

        }
        
    }
}
