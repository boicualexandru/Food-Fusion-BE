USE [master]
BACKUP LOG [FoodFusion] TO  DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\Backup\FoodFusion_LogBackup_2018-11-25_10-50-58.bak' WITH NOFORMAT, NOINIT,  NAME = N'FoodFusion_LogBackup_2018-11-25_10-50-58', NOSKIP, NOREWIND, NOUNLOAD,  NORECOVERY ,  STATS = 5
RESTORE DATABASE [FoodFusion] FROM  DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\Backup\FoodFusion.bak' WITH  FILE = 1,  NOUNLOAD,  STATS = 5

GO


