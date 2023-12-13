using Microsoft.EntityFrameworkCore;

namespace InsecureProject.Database;

public class DbModelContext : DbContext
{
    public DbSet<Forecast> Forecasts { get; set; }

    public string DbPath { get; }

    public DbModelContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, "forecast.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}

public class Forecast
{
    public int ForecastId { get; set; }
    public string City { get; set; }
    public string Summary { get; set; }
    public int Temperature { get; set; }
}
