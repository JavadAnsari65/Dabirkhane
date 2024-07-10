using Microsoft.EntityFrameworkCore;

public class Context : DbContext
{
    public DbSet<Atteched> Attecheds_tbl { get; set; }
    public DbSet<Messages> Messages_tbl { get; set; }
    public DbSet<Recivers> Recivers_tbl { get; set; }
    public DbSet<Users> Users_tbl { get; set; }
    public DbSet<smsUser> sms_tbl { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=./;Initial Catalog=IliaDabirkhane;User Id=sa;Password=12345@Iran;MultipleActiveResultSets=true;TrustServerCertificate=true;");
    }
}