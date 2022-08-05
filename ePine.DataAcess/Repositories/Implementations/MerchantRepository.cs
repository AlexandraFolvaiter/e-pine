using ePine.DataAccess.Connections;
using ePine.DataAccess.Entities;
using ePine.DataAccess.Repositories.Contracts;

namespace ePine.DataAccess.Repositories.Implementations;

public class MerchantRepository : BaseRepository, IMerchantRepository
{
    public MerchantRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public IQueryable<Merchant> GetAll()
    {
        return Context
            .Merchants
            .Where(m => m.IsPublic == true);
    }

    public Merchant? GetById(Guid id)
    {
        return Context
            .Merchants
            .FirstOrDefault(m => m.Id == id && m.IsPublic == true);
    }
}