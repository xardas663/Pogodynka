using Microsoft.EntityFrameworkCore;
using Pogodynka.Core.Domain;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pogodynka.Infrastructure.EF
{
    public class ImagesContext:DbContext
    {
        private readonly SqlSettings _sqlSettings;
        public DbSet<Images> Images { get; set; }
        public ImagesContext(DbContextOptions<ImagesContext> options, SqlSettings sqlSettings)
            :base(options)
        {
            _sqlSettings = sqlSettings;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_sqlSettings.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var photoConditionsBuilder = modelBuilder.Entity<Images>();
            photoConditionsBuilder.HasKey(x => x.Id);
            
        }

    }
}
