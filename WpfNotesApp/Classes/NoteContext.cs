using Microsoft.EntityFrameworkCore;

namespace WpfNotesApp.Classes
{
    public class NoteContext : DbContext
    {
        public DbSet<Note> Notes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var path = "path-to-your-database\\notes.db";
            optionsBuilder.UseSqlite($"Data Source={path}");
        }
    }
}
