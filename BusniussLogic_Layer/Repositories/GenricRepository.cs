using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusniussLogic_Layer.Repositories
{
    public class GenricRepository<Tentity> : IGenaricRepository<Tentity> where Tentity : class
    {
        protected DataContext context;
        protected DbSet<Tentity> dbset;
        

        public GenricRepository(DataContext context)
        {
            this.context = context;
            dbset=context.Set<Tentity>();
        }

        public void Create(Tentity entity)
        {
             dbset.Add(entity);
        }

        public void Delete(Tentity entity)
        {
            dbset.Remove(entity);
        }

        public Tentity? Get(int id) => dbset.Find(id);
        public IEnumerable<Tentity>? GetAll()=>dbset.ToList();
       

        public void Update(Tentity entity)
        {
            dbset.Update(entity);
        }
    }
}
