using Microsoft.EntityFrameworkCore;

namespace WpfNotesApp.Classes
{
    public class NoteContext : DbContext
    {
        public DbSet<Note> Notes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var path = "C:\\Users\\marku\\source\\repos\\WpfNotesApp\\notes.db";
            optionsBuilder.UseSqlite($"Data Source={path}");
        }
    }
}
