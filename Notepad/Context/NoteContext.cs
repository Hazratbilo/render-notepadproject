using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Notepad.Models;

namespace Notepad.Context
{
  
    public class NoteContext : DbContext
    {
      
        public NoteContext(DbContextOptions<NoteContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseNpgsql("Host=localhost;Port=5432;Database=Notepad;Username=postgres;Password=Alatoye2922;")
                .ConfigureWarnings(w =>
                    w.Ignore(RelationalEventId.PendingModelChangesWarning));
        }


        public DbSet<Note> Notes { get; set; }
    }
}
