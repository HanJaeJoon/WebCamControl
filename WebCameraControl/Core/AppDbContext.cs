using Microsoft.EntityFrameworkCore;
using WebCameraControl.Models;

namespace WebCameraControl.Core;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<UserFile> UserFiles => Set<UserFile>();

    public DbSet<AppInfo> AppInfo => Set<AppInfo>();
}
