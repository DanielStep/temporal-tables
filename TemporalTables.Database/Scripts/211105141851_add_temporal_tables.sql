/* Migration Script 211105141851_add_temporal_tables.sql */

--Create temporal table with anonymous history table
------------------------------------------------------------------------------------------

CREATE TABLE TempTableWithAnon
(
    ID INT NOT NULL PRIMARY KEY CLUSTERED
  , Name VARCHAR(50) NOT NULL
  , SysStartTime DATETIME2 GENERATED ALWAYS AS ROW START NOT NULL
  , SysEndTime DATETIME2 GENERATED ALWAYS AS ROW END NOT NULL
  , PERIOD FOR SYSTEM_TIME (SysStartTime,SysEndTime)
)
WITH (SYSTEM_VERSIONING = ON);

GO

INSERT INTO [dbo].[TempTableWithAnon]
           ([ID]
           ,[Name])
     VALUES(
           1,
           'test1')
GO


update TempTableWithAnon set Name = 'test2' where ID = 1
update TempTableWithAnon set Name = 'test3' where ID = 1
GO

--Create Temporal Table with a default system generate history able withs specified name
------------------------------------------------------------------------------------------

CREATE TABLE TempTableWithDefault
(
    ID INT NOT NULL PRIMARY KEY CLUSTERED
  , Name VARCHAR(50) NOT NULL
  , SysStartTime DATETIME2 GENERATED ALWAYS AS ROW START NOT NULL
  , SysEndTime DATETIME2 GENERATED ALWAYS AS ROW END NOT NULL
  , PERIOD FOR SYSTEM_TIME (SysStartTime,SysEndTime)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.TempTableWithDefaultHistory));
GO

INSERT INTO [dbo].[TempTableWithDefault]
           ([ID]
           ,[Name])
     VALUES
           (1,
           'test1')
GO

update TempTableWithDefault set Name = 'test2' where ID = 1
update TempTableWithDefault set Name = 'test3' where ID = 1

--Change existing regular table to temporal table
------------------------------------------------------------------------------------------

CREATE TABLE TempTableExisting
(
    ID INT NOT NULL PRIMARY KEY CLUSTERED
  , Name VARCHAR(50) NOT NULL
)
GO

INSERT INTO [dbo].[TempTableExisting]
           ([ID]
           ,[Name])
     VALUES
           (1,
           'test1')
GO

ALTER TABLE TempTableExisting
    ADD
        SysStartTime DATETIME2 GENERATED ALWAYS AS ROW START
            CONSTRAINT DF_SysStart DEFAULT SYSUTCDATETIME()
      , SysEndTime DATETIME2 GENERATED ALWAYS AS ROW END
            CONSTRAINT DF_SysEnd DEFAULT CONVERT(DATETIME2, '9999-12-31 23:59:59.9999999'),
        PERIOD FOR SYSTEM_TIME (SysStartTime, SysEndTime);
GO


ALTER TABLE TempTableExisting
    SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.TempTableExistingHistory));
GO

update TempTableExisting set Name = 'test2' where ID = 1
update TempTableExisting set Name = 'test3' where ID = 1

GO
