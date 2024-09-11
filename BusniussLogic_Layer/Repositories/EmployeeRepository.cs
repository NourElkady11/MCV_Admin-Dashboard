

using Microsoft.EntityFrameworkCore;

namespace BusniussLogic_Layer.Repositories
{
    public class EmployeeRepository : GenricRepository<Employee>, IEmployeeRepoistory
    {
      

        public EmployeeRepository(DataContext dbContext):base(dbContext)
        {
          
        }

        public IEnumerable<Employee> GetAll(string name)
        {
            return dbset.Where(e => e.Name == name);
        }
    }
}
