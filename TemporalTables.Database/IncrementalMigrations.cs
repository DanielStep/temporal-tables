using DbUp;
using DbUp.Engine;
using System;
using System.Linq;
using System.Reflection;

namespace TemporalTables.Database
{
    public class IncrementalMigrations
    {
        private readonly string _connectionString;

        public IncrementalMigrations(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DatabaseUpgradeResult Run(string[] args)
        {
            var engine = BuildMigrationEngine(args);

            return RunMigrations(args, engine);
        }

        private UpgradeEngine BuildMigrationEngine(string[] args)
        {
            var engineBuilder = DeployChanges.To
                .SqlDatabase(_connectionString)
                .JournalToSqlTable("dbo", "_DbUpSchemaChanges")
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(),
                    s => !s.Contains("EverytimeScript_"))
                .LogToConsole()
                .LogScriptOutput();

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

        private DatabaseUpgradeResult RunMigrations(string[] args, UpgradeEngine engine)
        {
            //Functionality to create SQL Object Definitions not included in dbup-core yet
            return engine.PerformUpgrade();
        }
    }
}