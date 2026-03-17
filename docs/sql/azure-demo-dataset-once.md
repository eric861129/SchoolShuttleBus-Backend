# Azure Demo Dataset Once

This script appends a larger demo dataset to the current Azure SQL database without changing application seed behavior.

File:
- `docs/sql/azure-demo-dataset-once.sql`

What it inserts:
- 1 administrator account
- 2 teacher accounts
- 5 parent accounts
- 5 student accounts
- staff profiles, guardians, routes, route stops
- student-guardian links
- route assignments
- one school week of ride registrations
- attendance sessions and attendance records
- one dispatch override
- notification template, notification job, deliveries, and a broadcast announcement

Shared password for all imported accounts:
- `P@ssw0rd!`

Recommended execution:

```powershell
$publishSettingsPath = 'C:\path\to\app-school-shuttlebus-demo.PublishSettings'
[xml]$xml = Get-Content $publishSettingsPath
$cs = $xml.publishData.publishProfile[0].SQLServerDBConnectionString
$server = [regex]::Match($cs, 'Server=tcp:([^,;]+)').Groups[1].Value
$db = [regex]::Match($cs, 'Initial Catalog=([^;]+)').Groups[1].Value
$user = [regex]::Match($cs, 'User ID=([^;]+)').Groups[1].Value
$pwd = [regex]::Match($cs, 'Password=([^;]+)').Groups[1].Value
sqlcmd -S "tcp:$server,1433" -d $db -U $user -P $pwd -i "docs\sql\azure-demo-dataset-once.sql"
```

Suggested login accounts for Swagger:
- administrator: `E0001`
- teacher: `T0001`
- parent: `0910-200-001`
- student: `S20001`

Post-import checks:
- `GET /swagger/index.html`
- `POST /api/auth/login`
- `GET /api/auth/me`
- admin endpoints with the administrator token
- attendance endpoints with a teacher token
- registration endpoints with a parent or student token
