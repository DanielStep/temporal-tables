using DbUp;
using DbUp.Engine;
using TemporalTables.Database;
using System;
using System.Linq;

namespace TemporalTables.Database
{
class Program
    {
        static int Main(string[] args)
        {
            var connectionString = GetConnectionString(args);

            if (args.Any(a => a.Equals("--ensureDbExists", StringComparison.InvariantCultureIgnoreCase)))
            {
                EnsureDatabase.For.SqlDatabase(connectionString);
            }

            DatabaseUpgradeResult everyTimeResult = null;
            try
            {
                var incrementalResult = new IncrementalMigrations(connectionString).Run(args);

                if (incrementalResult.Successful)
                {
                    everyTimeResult = new EverytimeMigrations(connectionString).Run(args);
                }

                return HandleResult(incrementalResult, everyTimeResult);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Incremental migrations error: {e}");
                Console.ResetColor();

                return -1;
            }
        }

        private static string GetConnectionString(string[] args)
        {
            var connectionStringArgument = args.SingleOrDefault(a => a.StartsWith("--connectionString", StringComparison.InvariantCultureIgnoreCase));

            return connectionStringArgument != null ? connectionStringArgument.Substring("--connectionString=".Length) : "Server=(local);Database=temporaltable-tests;Integrated Security=true;";
        }

        private static int HandleResult(DatabaseUpgradeResult incrementalResult, DatabaseUpgradeResult everytimeResult)
        {
            if (incrementalResult != null && !incrementalResult.Successful || everytimeResult != null && !everytimeResult.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Incremental migrations error: {incrementalResult?.Error}");
                Console.WriteLine($"Every time migrations error: {everytimeResult.Error}");
                Console.ResetColor();
                return -1;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();
            return 0;
        }
    }
}