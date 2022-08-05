using ePine.Business.Contracts;
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

        var models = merchantsEntity.Select(m => new MerchantModel
        {
            Id = m.Id,
            Name = m.Name,
            Description = m.Description,
            AccessToken = m.AccessToken,
            IsPublic = m.IsPublic
        });

        return models;
    }

    public MerchantModel GetMerchant(Guid id)
    {
        var merchantEntity = MerchantRepository.GetById(id);

        if (merchantEntity == null)
        {
            // TODO: return message
            return null;
        }

        var model =  new MerchantModel
        {
            Id = merchantEntity.Id,
            Name = merchantEntity.Name,
            Description = merchantEntity.Description,
            AccessToken = merchantEntity.AccessToken,
            IsPublic = merchantEntity.IsPublic
        };

        return model;
    }
}