
namespace BusniussLogic_Layer.Repositories
{
    public interface IEmployeeRepoistory:IGenaricRepository<Employee>
    {
        public IEnumerable<Employee> GetAllEmployees(string name);


        public IEnumerable<Employee> GetAllWithDepartment();
    }
}
