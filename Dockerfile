FROM mcr.microsoft.com/dotnet/sdk:9.0@sha256:3fcf6f1e809c0553f9feb222369f58749af314af6f063f389cbd2f913b4ad556 AS build
WORKDIR /app

COPY ./src/Biblioteca.Api/Biblioteca.Api.csproj .
COPY ./src/Biblioteca.Application/Biblioteca.Application.csproj .
COPY ./src/Biblioteca.Data/Biblioteca.Data.csproj .
COPY ./src/Biblioteca.Domain/Biblioteca.Domain.csproj .
RUN dotnet restore Biblioteca.Api.csproj

COPY . .
RUN dotnet publish ./src/Biblioteca.Api/Biblioteca.Api.csproj --no-restore -c Release -r linux-x64 -o out 

FROM mcr.microsoft.com/dotnet/aspnet:9.0@sha256:b4bea3a52a0a77317fa93c5bbdb076623f81e3e2f201078d89914da71318b5d8
WORKDIR /app
EXPOSE 8080

COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Biblioteca.Api.dll"]