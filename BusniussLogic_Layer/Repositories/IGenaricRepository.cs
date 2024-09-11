using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusniussLogic_Layer.Repositories
{
    public interface IGenaricRepository<TEntity>
    {
        int Create(TEntity entity);
        int Delete(TEntity entity);
        TEntity? Get(int id);
        IEnumerable<TEntity>? GetAll();
        int Update(TEntity entity);
    }
}
