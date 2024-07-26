using Microsoft.EntityFrameworkCore;
using URLShortening.Entites;
using URLShortening.Services;

namespace URLShortening

{
    public class ApplicationDBContext : DbContext

    {
        public ApplicationDBContext(DbContextOptions options)
            : base(options)
        {

        }
        public DbSet<ShortenURL> ShortenURL { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShortenURL>(builder =>
            {
                builder.Property(s => s.Code).HasMaxLength(URLShorteningServices.NumberOfcharshort);
                builder.HasIndex(s=>s.Code).IsUnique();

            });

        }
    }
}
