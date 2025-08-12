using GymExerciseTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace GymExerciseTracker.Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<GymSession> GymSessions { get; set; }
}

