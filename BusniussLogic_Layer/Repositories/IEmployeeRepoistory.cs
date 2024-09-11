
namespace BusniussLogic_Layer.Repositories
{
    public interface IEmployeeRepoistory:IGenaricRepository<Employee>
    {
        public IEnumerable<Employee> GetAll(string name);
    }
}
