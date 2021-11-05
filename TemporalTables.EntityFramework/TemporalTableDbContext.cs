using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemporalTables.EntityFramework
{
    public class TemporalTableDbContext : DbContext
    {
        public DbSet<TempTableExisting> TempTable { get; set; }
        public DbSet<EntityFrameworkSpecific> EntityFrameworkSpecificTable { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(local);Database=temporaltable-tests;Integrated Security=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<TempTableExisting>()
                .ToTable("TempTableExisting", b => b.IsTemporal(
                        options =>
                        {
                            options.HasPeriodStart("SysStartTime");
                            options.HasPeriodEnd("SysEndTime");
                            options.UseHistoryTable("TempTableExistingHistory");
                        }
                    ));

            modelBuilder
                .Entity<EntityFrameworkSpecific>()
                .ToTable("EntityFrameworkSpecific", b => b.IsTemporal(
                        options => {}
                    ));
        }

        public class TempTableExisting
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }

        public class EntityFrameworkSpecific
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }
    }
}
