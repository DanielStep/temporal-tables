using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace TemporalTables.EntityFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new TemporalTableDbContext())
            {
                var historicalRecords = context.TempTable
                    .TemporalAll()
                    .OrderBy(entity => EF.Property<DateTime>(entity, "SysStartTime"))
                    .Select(entity =>
                        new
                        {
                            Entity = entity,
                            PeriodStart = EF.Property<DateTime>(entity, "SysStartTime"),
                            PeriodEnd = EF.Property<DateTime>(entity, "SysEndTime")
                        })
                    .ToList();

                var historicalRecords2 = context.EntityFrameworkSpecificTable
                    .TemporalAll()
                    .OrderBy(entity => EF.Property<DateTime>(entity, "PeriodStart"))
                    .Select(entity =>
                        new
                        {
                            Entity = entity,
                            PeriodStart = EF.Property<DateTime>(entity, "PeriodStart"),
                            PeriodEnd = EF.Property<DateTime>(entity, "PeriodEnd")
                        })
                    .ToList();

                var currentName = context.EntityFrameworkSpecificTable.Single(x => x.ID == 1).Name;
                Console.WriteLine($"Current name: {currentName}");
                var nameChange = context.EntityFrameworkSpecificTable.Where(x => x.ID == 1 && x.Name == "test1").ToList();
                Console.WriteLine($"Old name found? {nameChange.Any()}");

                nameChange = context.EntityFrameworkSpecificTable.TemporalAll().Where(x => x.ID == 1 && x.Name == "test1").ToList();
                Console.WriteLine($"Old name found with temporal table? {nameChange.Any()}");
            }
        }
    }
}
