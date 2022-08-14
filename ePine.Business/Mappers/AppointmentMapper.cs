using ePine.DataAccess.Entities;
using ePine.Models;

namespace ePine.Business.Mappers;

public static class AppointmentMapper
{
    // public static Appointment ToEntity(this AppointmentModel model)
    // {
    //     return new Appointment(model.UserId, model.MerchantId, model.AppointmentId, model.Name, model.DateAndTime);
    // }

    public static AppointmentModel ToModel(this Appointment entity)
    {
        return new AppointmentModel
        {
            Id = entity.Id,
            UserId = entity.UserId,
            MerchantId = entity.MerchantId,
            AppointmentId = entity.AppointmentId,
            Name = entity.Name,
            DateAndTime = entity.DateAndTime
        };
    }
}