using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccess.Repository.IRepository
{
    public interface IShoppingCartRespository : IRepository<ShoppingCart>
    {
        int IncreamentCount(ShoppingCart shoppingCart, int count);
        int DecreamentCount(ShoppingCart shoppingCart,int count);
        
    }
}
