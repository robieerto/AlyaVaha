sqllocaldb create AlyaDB
sqllocaldb start AlyaDB
sqlcmd -S (localdb)\AlyaDB -Q "ALTER LOGIN sa ENABLE; ALTER LOGIN sa WITH PASSWORD = 'MuL7J58B6ftSWkaESXLN';"
for /f "tokens=*" %%i in ('whoami') do set "user=%%i"
sqlcmd -S (localdb)\AlyaDB -U sa -P MuL7J58B6ftSWkaESXLN -Q "DROP LOGIN [%user%];"