#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS test
WORKDIR /src
COPY ["TweetsService/TweetsService.csproj", "TweetsService/"]
RUN dotnet restore "./TweetsService/./TweetsService.csproj"
COPY . .
WORKDIR "/src/TweetsService"
RUN dotnet test "./TweetsService.csproj" -c Release --no-build  

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TweetsService/TweetsService.csproj", "TweetsService/"]
RUN dotnet restore "./TweetsService/./TweetsService.csproj"
COPY . .
WORKDIR "/src/TweetsService"
RUN dotnet build "./TweetsService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TweetsService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TweetsService.dll"]