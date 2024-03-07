FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Novle.API/Novle.API.csproj", "Novle.API/"]
COPY ["Novle.Application/Novle.Application.csproj", "Novle.Application/"]
COPY ["Novle.Domain/Novle.Domain.csproj", "Novle.Domain/"]
COPY ["Novle.Infrastructure/Novle.Infrastructure.csproj", "Novle.Infrastructure/"]
RUN dotnet restore "Novle.API/Novle.API.csproj"
COPY . .
WORKDIR "/src/Novle.API"
RUN dotnet build "Novle.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Novle.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
COPY --from=build /src/Novle.API/https/* /app/publish/https/

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Novle.API.dll"]
