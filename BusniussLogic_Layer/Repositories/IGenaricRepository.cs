using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusniussLogic_Layer.Repositories
{
    public interface IGenaricRepository<TEntity>
    {
        void Create(TEntity entity);
        void Delete(TEntity entity);
        TEntity? Get(int id);
        IEnumerable<TEntity>? GetAll();
        void Update(TEntity entity);
    }
}
