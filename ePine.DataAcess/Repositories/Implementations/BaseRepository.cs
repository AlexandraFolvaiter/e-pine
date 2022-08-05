using ePine.DataAccess.Connections;

namespace ePine.DataAccess.Repositories.Implementations;

public class BaseRepository
{
    protected ApplicationDbContext Context;

    public BaseRepository(ApplicationDbContext context)
    {
        Context = context;
    }
}