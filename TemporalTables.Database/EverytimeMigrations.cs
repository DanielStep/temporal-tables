using DbUp;
using DbUp.Engine;
using DbUp.Helpers;
using System;
using System.Linq;
using System.Reflection;

namespace TemporalTables.Database
{
    public class EverytimeMigrations
    {
        private readonly string _connectionString;

        public EverytimeMigrations(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DatabaseUpgradeResult Run(string[] args)
        {
            var engine = BuildMigrationEngine(args);

            return engine.PerformUpgrade();
        }

        private UpgradeEngine BuildMigrationEngine(string[] args)
        {
            var engineBuilder = DeployChanges.To.SqlDatabase(_connectionString).WithScriptsEmbeddedInAssembly(
                    Assembly.GetExecutingAssembly(), s => s.Contains("EverytimeScript_"))
                .JournalTo(new NullJournal()).LogToConsole().LogScriptOutput();

            if (args.Any(a => a.Equals("--withTransaction", StringComparison.InvariantCultureIgnoreCase)))
            {
                engineBuilder = engineBuilder.WithTransaction();
            }

            else if (args.Any(a => a.Equals("--withTransactionPerScript", StringComparison.InvariantCultureIgnoreCase)))
            {
                engineBuilder = engineBuilder.WithTransactionPerScript();
            }

            return engineBuilder.Build();
        }
    }
}