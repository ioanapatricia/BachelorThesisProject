$api_list = @(
	'../FoodOrderingBackend/BackendForFrontend.Api'
	'../FoodOrderingBackend/ProductManagement.Api'
	'../FoodOrderingBackend/Ordering.Api'
	'../FoodOrderingBackend/SiteManagement.Api'
)

$images = @(
	'ioanapatricia/bff.api:v1.0'
	'ioanapatricia/pm.api:v1.0'
	'ioanapatricia/ord.api:v1.0'
	'ioanapatricia/sm.api:v1.0'
	'ioanapatricia/adm.client:v1.0'
	'ioanapatricia/ctm.client:v1.0'
)
$containers = ('bff.api','bff.db','pm.api','pm.db','ord.api','sm.api', 'adm.client', 'ctm.client')

Write-Host "Removing all docker containers. (if this fails it means you don't have them, and that's ok)" -fore green
docker rm $containers -f

Write-Host "Removing all docker images. (if this fails it means you don't have them, and that's ok)" -fore green
docker rmi $images

Write-Host "Republishing .NET 5 projects in order to apply updates..." -fore green
foreach ($api in $api_list) {
	dotnet publish $api
}

#region UnitTesting
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
#endregion UnitTesting

& $PSScriptRoot\uniTest.ps1

Write-Host "Copying files for seed in PM.API..." -fore green
Copy-Item '../FoodOrderingBackend/ProductManagement.Api/Helpers/DataForSeed/ProductImages' '../FoodOrderingBackend/ProductManagement.Api/bin/Debug/net5.0/Helpers/DataForSeed' -Force -Recurse

Write-Host "Composing new image from ./docker-compose-dev-build.yaml" -fore green
docker-compose -f docker-compose-build.yaml up -d

write-host "stopping running containers" -fore green
docker stop $containers

write-host "overwriting images on docker hub..." -fore green
docker logout
docker login -u ioanapatricia -p Necrophagist!

foreach ($img in $images) {
	docker push $img
}

read-host -prompt "press any key to close...";

