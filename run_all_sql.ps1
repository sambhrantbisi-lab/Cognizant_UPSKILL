<#
Run all SQL scripts for CompanyDB and verify results.
Usage:
  .\run_all_sql.ps1 -MySqlUser root -MySqlPassword "yourpassword"
If you omit -MySqlPassword the script will try to run mysql without a password (prompts may occur).
#>
param(
  [string]$MySqlUser = 'root',
  [string]$MySqlPassword = ''
)

function Check-MySqlCmd {
  if (Get-Command mysql -ErrorAction SilentlyContinue) { return $true }
  Write-Error "mysql client not found in PATH. Install MySQL client or add to PATH."
  return $false
}

if (-not (Check-MySqlCmd)) { exit 1 }

$passArg = ''
if ($MySqlPassword -ne '') { $passArg = "--password=$MySqlPassword" }

Write-Host "Running schema.sql..."
Get-Content "C:\Users\priyanshu\SQL\schema.sql" | mysql -u $MySqlUser $passArg
if ($LASTEXITCODE -ne 0) { Write-Error "schema.sql failed (exit $LASTEXITCODE)"; exit $LASTEXITCODE }

Write-Host "Running insert_data.sql..."
Get-Content "C:\Users\priyanshu\SQL\insert_data.sql" | mysql -u $MySqlUser $passArg
if ($LASTEXITCODE -ne 0) { Write-Error "insert_data.sql failed (exit $LASTEXITCODE)"; exit $LASTEXITCODE }

Write-Host "Verifying counts..."
mysql -u $MySqlUser $passArg -e "USE CompanyDB; SELECT 'Departments', COUNT(*) FROM Departments; SELECT 'Employees', COUNT(*) FROM Employees; SELECT 'Projects', COUNT(*) FROM Projects; SELECT 'Employee_Project', COUNT(*) FROM Employee_Project;"

Write-Host "Running queries.sql (outputs to queries_output.txt)..."
Get-Content "C:\Users\priyanshu\SQL\queries.sql" | mysql -u $MySqlUser $passArg CompanyDB > "C:\Users\priyanshu\SQL\queries_output.txt"

Write-Host "Running advanced_queries.sql (outputs to advanced_output.txt)..."
Get-Content "C:\Users\priyanshu\SQL\advanced_queries.sql" | mysql -u $MySqlUser $passArg CompanyDB > "C:\Users\priyanshu\SQL\advanced_output.txt"

Write-Host "Done. Outputs saved to queries_output.txt and advanced_output.txt in the same folder."
Write-Host "If any command prompts for a password, re-run the script with -MySqlPassword parameter."
