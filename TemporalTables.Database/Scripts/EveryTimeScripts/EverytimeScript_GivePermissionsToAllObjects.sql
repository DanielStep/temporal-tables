--Give permissions to user and role based on object type 
SET NoCount ON
DECLARE @tbl as TABLE(sql nvarchar(2000), id int identity primary key)
INSERT INTO @tbl (sql)
SELECT 'GRANT SELECT, INSERT, UPDATE, DELETE ON [dbo].['+ [name] + '] TO User AS DBO' FROM [sys].[objects] WHERE [type]='U' AND [schema_id] = 1

DECLARE @m INT = SCOPE_IDENTITY(), @c INT = 1, @s AS NVARCHAR(2000)

WHILE @c <= @m
BEGIN
SELECT @s = sql FROM @tbl WHERE Id = @c
    EXEC(@s)

SET @c+=1
END