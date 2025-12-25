# Use the official .NET 8.0 runtime image as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the official .NET 8.0 SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project files
COPY ["Api/Api.csproj", "Api/"]
COPY ["Api.UnitTests/Api.UnitTests.csproj", "Api.UnitTests/"]
COPY ["Api.IntegrationTests/Api.IntegrationTests.csproj", "Api.IntegrationTests/"]

# Restore dependencies
RUN dotnet restore "Api/Api.csproj"

# Copy the entire source code
COPY . .

# Build the application
WORKDIR "/src/Api"
RUN dotnet build "Api.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Create the final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set the entry point
ENTRYPOINT ["dotnet", "Api.dll"]
