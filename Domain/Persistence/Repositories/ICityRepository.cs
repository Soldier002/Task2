using Domain.Persistence.Entities;

namespace Domain.Persistence.Repositories
{
    public interface ICityRepository
    {
        Task InsertIfNotExists(City city);

        Task<IList<City>> GetAll();
    }
}