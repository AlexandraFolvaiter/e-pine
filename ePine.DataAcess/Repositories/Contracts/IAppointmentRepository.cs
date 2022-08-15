using ePine.DataAccess.Entities;

namespace ePine.DataAccess.Repositories.Contracts;

public interface IAppointmentRepository
{
    IQueryable<Appointment> GetAll(Guid userId);

    Appointment GetAppointment(Guid appointmentId);

    void AddAppointment(Appointment appointment);

    void RemoveAppointment(Appointment appointment);
}