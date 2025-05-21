FROM mcr.microsoft.com/dotnet/sdk:9.0@sha256:3fcf6f1e809c0553f9feb222369f58749af314af6f063f389cbd2f913b4ad556 AS build
WORKDIR /app

COPY . ./
RUN dotnet restore

FROM build AS migrations
RUN dotnet tool install --version 9.0.5 --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"
ENTRYPOINT dotnet-ef database update --project src/Biblioteca.Data/ --startup-project src/Biblioteca.Api/

FROM build AS publish
RUN dotnet publish ./src/Biblioteca.Api/Biblioteca.Api.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0@sha256:b4bea3a52a0a77317fa93c5bbdb076623f81e3e2f201078d89914da71318b5d8
WORKDIR /app
COPY --from=publish /app/out .
ENTRYPOINT ["dotnet", "Biblioteca.Api.dll"]