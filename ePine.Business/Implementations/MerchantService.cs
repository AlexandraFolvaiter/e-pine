using ePine.Business.Contracts;
using ePine.Business.Mappers;
using ePine.DataAccess.Repositories.Contracts;
using ePine.Models;

namespace ePine.Business.Implementations;

public class MerchantService : BaseService, IMerchantService
{
    public MerchantService(IMerchantRepository merchantRepository) : base(merchantRepository)
    {
    }

    public IEnumerable<MerchantModel> GetMerchants()
    {
        var merchantsEntity = MerchantRepository.GetAll().ToList();

        var models = merchantsEntity.Select(m => m.ToModel());

        return models;
    }

    public IEnumerable<MerchantModel> GetMerchantsByUser(Guid userId)
    {
        var merchantsEntity = MerchantRepository
            .GetAll()
            .Where(m => m.AdminId == userId)
            .ToList();

        var models = merchantsEntity.Select(m => m.ToModel());

        return models;
    }

    public MerchantModel GetMerchant(Guid id)
    {
        var merchantEntity = MerchantRepository.GetById(id);

        return merchantEntity == null
            ? null 
            : merchantEntity.ToModel();
    }

    public void AddMerchant(Guid userId, MerchantModel model)
    {
        var entity = model.ToEntity(userId);
        
        MerchantRepository.Add(entity);
    }
}