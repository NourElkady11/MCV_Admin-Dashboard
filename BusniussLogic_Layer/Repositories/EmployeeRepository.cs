
using Microsoft.EntityFrameworkCore;

namespace BusniussLogic_Layer.Repositories
{
    public class EmployeeRepository : GenricRepository<Employee>, IEmployeeRepoistory
    {
      

        public EmployeeRepository(DataContext dbContext):base(dbContext)
        {
          
        }

        public IEnumerable<Employee> GetAllEmployees(string name)
        {
            return dbset.Where(e => e.Name.ToLower().Contains(name.ToLower())).Include(d=>d.Department).ToList();
        }

        public IEnumerable<Employee> GetAllWithDepartment()=>dbset.Include(e => e.Department).ToList();
    }
}
