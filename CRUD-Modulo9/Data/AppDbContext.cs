using CRUD_Modulo9.Entities;
using Microsoft.EntityFrameworkCore;

namespace CRUD_Modulo9.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    public DbSet<Empleado> Empleado { get; set; }

    public DbSet<Usuario> Usuarios { get; set; }
}
