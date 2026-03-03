FROM mcr.microsoft.com/dotnet/aspnet:5.0

WORKDIR /app
COPY bin/Debug/net5.0 .

ENTRYPOINT ["dotnet", "Ordering.API.dll"]

ENV ASPNETCORE_URLS=http://+:82