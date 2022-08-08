using ePine.DataAccess.Connections;
using ePine.DataAccess.Entities;
using ePine.DataAccess.Repositories.Contracts;

namespace ePine.DataAccess.Repositories.Implementations;

public class AppointmentRepository : BaseRepository, IAppointmentRepository
{
    public AppointmentRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public IQueryable<Appointment> GetAll(Guid userId)
    {
        return Context.Appointments.Where(a => a.UserId == userId);
    }

    public Appointment GetAppointment(Guid appointmentId)
    {
        return Context.Appointments.FirstOrDefault(a => a.Id == appointmentId);
    }

    public void AddAppointment(Appointment appointment)
    {
        Context.Appointments.Add(appointment);
        Context.SaveChanges();
    }

    public void RemoveAppointment(Appointment appointment)
    {
        Context.Appointments.Remove(appointment);
        Context.SaveChanges();
    }
}