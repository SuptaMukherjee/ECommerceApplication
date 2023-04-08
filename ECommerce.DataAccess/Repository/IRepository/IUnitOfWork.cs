using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRespositoy Category { get;  }

        ICoverTypeRepository CoverType { get; }

        IProductRespository Product { get; }
        void Save();
    }
}
