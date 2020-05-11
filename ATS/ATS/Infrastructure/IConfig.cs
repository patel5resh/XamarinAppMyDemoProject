using SQLite;

namespace ATS.Infrastructure
{
    public interface IConfig
    {
        SQLiteConnection DBConnect();
    }
}
