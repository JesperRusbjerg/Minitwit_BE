#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Minitwit_BE.Api/Minitwit_BE.Api.csproj", "Minitwit_BE.Api/"]
COPY ["Minitwit_BE.DomainService/Minitwit_BE.DomainService.csproj", "Minitwit_BE.DomainService/"]
COPY ["Minitwit_BE.Persistence/Minitwit_BE.Persistence.csproj", "Minitwit_BE.Persistence/"]
COPY ["Minitwit_BE.Domain/Minitwit_BE.Domain.csproj", "Minitwit_BE.Domain/"]
RUN dotnet restore --no-cache "Minitwit_BE.Api/Minitwit_BE.Api.csproj"
COPY . .
WORKDIR "/src/Minitwit_BE.Api"
RUN dotnet build "Minitwit_BE.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Minitwit_BE.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Minitwit_BE.Api.dll", "--urls", "http://*:5000;http://*:5001"]
