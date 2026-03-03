Write-Host "Removing all docker containers. (if this fails it means you don't have them, and that's ok)" -fore green
$containers = ('bff.api','bff.db','pm.api','pm.db','ord.api','sm.api', 'adm.client', 'ctm.client')
docker rm $containers -f



Write-Host "Removing all docker images. (if this fails it means you don't have them, and that's ok)" -fore green
$images = ('ioanapatricia/bff.api:v1.0','ioanapatricia/pm.api:v1.0', 'ioanapatricia/ord.api:v1.0', 'ioanapatricia/sm.api:v1.0', 'ioanapatricia/adm.client:v1.0', 'ioanapatricia/ctm.client:v1.0')
docker rmi $images


docker logout
docker login -u ioanapatricia -p Necrophagist!

write-host "composing new image from ./docker-compose-pull-remote.yaml" -fore green
docker-compose -f docker-compose-pull-remote.yaml up -d

write-host -prompt "press any key to close...";