#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Foundations/HC.Foundation.Common/HC.Foundation.Common.csproj", "Foundations/HC.Foundation.Common/"]
COPY ["Foundations/HC.Foundation.Data/HC.Foundation.Data.csproj", "Foundations/HC.Foundation.Data/"]
COPY ["Services/HC.Service.Authentication/HC.Service.Authentication.csproj", "Services/HC.Service.Authentication/"]
RUN dotnet restore "Services/HC.Service.Authentication/HC.Service.Authentication.csproj"
COPY . .
WORKDIR "/src/Services/HC.Service.Authentication"
RUN dotnet build "HC.Service.Authentication.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HC.Service.Authentication.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HC.Service.Authentication.dll"]