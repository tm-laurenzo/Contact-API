#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY *.sln .
COPY ContactAPI3.1/*.csproj ContactAPI3.1/

COPY Models/*.csproj Models/
COPY DataOperations/*.csprojData Operations/
COPY DTO/*.csproj DTO/
COPY Commons/*.csproj Commons/  
COPY Core/*.csproj Core/

RUN dotnet restore ContactAPI3.1/*.csproj

COPY . .
WORKDIR /src/ContactAPI3.1
RUN dotnet build
# RUN dotnet build "ContactAPI3.1.csproj" -c Release -o /app/build

FROM build AS publish
WORKDIR /src/ContactAPI3.1
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "ContactAPI3.1.dll"]
CMD ASPNETCORE_URLS=http://*:$PORT dotnet ContactAPI3.1.dll