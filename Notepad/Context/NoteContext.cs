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
                .UseNpgsql(" \"Host=dpg-d4necv7gi27c738hs540-a.oregon-postgres.render.com;Port=5432;Database=notepad_axz5;Username=notepad_axz5_user;Password=gdzZ0BUZIwUk9p9a0NBQuCbrAUB5hNDM;SSL Mode=Require;Trust Server Certificate=true\";")
                .ConfigureWarnings(w =>
                    w.Ignore(RelationalEventId.PendingModelChangesWarning));
        }


        public DbSet<Note> Notes { get; set; }
    }
}
