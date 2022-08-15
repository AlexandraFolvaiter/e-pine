using ePine.DataAccess.Entities;

namespace ePine.DataAccess.Repositories.Contracts;

public interface IMerchantRepository
{
    public IQueryable<Merchant> GetAll();

    public Merchant? GetById(Guid id);

    public void Add(Merchant entity);
}