#define parent location
$path = (Get-Location | Select-Object -ExpandProperty Path) -split '\\'
$parent_path = ($path | Where-Object {$_ -ne $path[-1]} ) -join '\'

$test_project_paths = Get-ChildItem -Path (-join($parent_path,'\','FoodOrderingBackend')) | Where-Object {$_.name -match 'Tests' } | Select-Object -ExpandProperty FullName

foreach($test_project in $test_project_paths){
    Write-Host -Object `n

    $project_name = ($test_project -split'\\')[-1]

    Write-Host -Object (-join('Now testing: ',$project_name)) -ForegroundColor 'Green'

    dotnet test $test_project

    if ($LASTEXITCODE -ne 0 ){
        Write-Host -Object (-join('Test project failed: ',$project_name)) -ForegroundColor 'Red'
        exit
    }

    Write-Host -Object (-join('Test project succeeded: ',$project_name)) -ForegroundColor 'Green'

    if($LASTEXITCODE) { Remove-Variable LASTEXITCODE }
}