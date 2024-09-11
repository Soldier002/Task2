using Domain.Persistence.Entities;

namespace Domain.Persistence.Repositories
{
    public interface ICityRepository
    {
        Task InsertIfNotExists(City city, CancellationToken ct);

        Task<IList<City>> GetAll();
    }
}