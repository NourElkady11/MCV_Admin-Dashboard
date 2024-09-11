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

        public int Create(Tentity entity)
        {
             dbset.Add(entity);
            return context.SaveChanges();
        }

        public int Delete(Tentity entity)
        {
            dbset.Remove(entity);
            return context.SaveChanges();
        }

        public Tentity? Get(int id) => dbset.Find(id);
        public IEnumerable<Tentity>? GetAll()=>dbset.ToList();
       

        public int Update(Tentity entity)
        {
            dbset.Update(entity);
            return context.SaveChanges();
        }
    }
}
