Write-Host "Removing all docker containers. (if this fails it means you don't have them, and that's ok)" -fore green
$containers = ('bff.api','bff.db','pm.api','pm.db','ord.api','sm.api', 'adm.client', 'ctm.client')
docker rm $containers -f

Write-Host "Removing all docker images. (if this fails it means you don't have them, and that's ok)" -fore green
$images = ('ioanapatricia/bff.api:v1.0','ioanapatricia/pm.api:v1.0', 'ioanapatricia/ord.api:v1.0', 'ioanapatricia/sm.api:v1.0', 'ioanapatricia/adm.client:v1.0', 'ioanapatricia/ctm.client:v1.0')
docker rmi $images


Write-Host "Republishing .NET 5 projects in order to apply updates..." -fore green
dotnet publish ../FoodOrderingBackend/BackendForFrontend.Api
dotnet publish ../FoodOrderingBackend/ProductManagement.Api
dotnet publish ../FoodOrderingBackend/Ordering.Api
dotnet publish ../FoodOrderingBackend/SiteManagement.Api

Write-Host "Copying files for seed in PM.API..." -fore green
Copy-Item ../FoodOrderingBackend/ProductManagement.Api/Helpers/DataForSeed/ProductImages ../FoodOrderingBackend/ProductManagement.Api/bin/Debug/net5.0/Helpers/DataForSeed -Force -Recurse


Write-Host "Composing new image from ./docker-compose-dev-build.yaml" -fore green
docker-compose -f docker-compose-build.yaml up -d

write-host "stopping running containers" -fore green
docker stop $containers

write-host "overwriting images on docker hub..." -fore green
docker logout
docker login -u ioanapatricia -p Necrophagist!
docker push ioanapatricia/bff.api:v1.0
docker push ioanapatricia/pm.api:v1.0
docker push ioanapatricia/ord.api:v1.0
docker push ioanapatricia/sm.api:v1.0
docker push ioanapatricia/adm.client:v1.0
docker push ioanapatricia/ctm.client:v1.0

read-host -prompt "press any key to close...";

