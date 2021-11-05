/* Migration Script 211105153720_EntityFramework-table.sql */
CREATE TABLE EntityFrameworkSpecific
(
    ID INT NOT NULL PRIMARY KEY CLUSTERED
  , Name VARCHAR(50) NOT NULL
  , PeriodStart DATETIME2 GENERATED ALWAYS AS ROW START NOT NULL
  , PeriodEnd DATETIME2 GENERATED ALWAYS AS ROW END NOT NULL
  , PERIOD FOR SYSTEM_TIME (PeriodStart,PeriodEnd)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.EntityFrameworkSpecificHistory));
GO

INSERT INTO [dbo].[EntityFrameworkSpecific]
           ([ID]
           ,[Name])
     VALUES
           (1,
           'test1')
GO

update EntityFrameworkSpecific set Name = 'test2' where ID = 1
update EntityFrameworkSpecific set Name = 'test3' where ID = 1