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
            }
        }
    }
}
