﻿using Microsoft.EntityFrameworkCore;
using Vale.Geographic.Infra.Data.Mappings;

namespace Vale.Geographic.Infra.Data.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AreaMap());
            modelBuilder.ApplyConfiguration(new PointOfInterestMap());
            modelBuilder.ApplyConfiguration(new RouteMap());
            modelBuilder.ApplyConfiguration(new SegmentMap());
            modelBuilder.ApplyConfiguration(new CategoryMap());
            modelBuilder.ApplyConfiguration(new AuditoryMap());
            modelBuilder.ApplyConfiguration(new FocalPointMap());
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new NotificationAnswerMap());
            modelBuilder.ApplyConfiguration(new SitesMap());
            modelBuilder.ApplyConfiguration(new SitesPerimetersMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}