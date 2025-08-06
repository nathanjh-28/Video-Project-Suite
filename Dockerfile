# syntax=docker/dockerfile:1

################################################################################
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copy csproj and restore dependencies
COPY Video-Project-Suite.Api/*.csproj Video-Project-Suite.Api/
RUN dotnet restore Video-Project-Suite.Api/

# Copy source code and build
COPY . .
WORKDIR /source/Video-Project-Suite.Api
RUN dotnet publish -c Release -o /app --no-restore

################################################################################
# Development stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS development
WORKDIR /app

# Copy the built app
COPY --from=build /app .

# Create data directory for SQLite
RUN mkdir -p /app/data

# Set environment
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:5001

EXPOSE 5001

ENTRYPOINT ["dotnet", "Video-Project-Suite.Api.dll"]

################################################################################
# Production stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS production
WORKDIR /app

# Copy the built app
COPY --from=build /app .

# Create data directory for SQLite
RUN mkdir -p /app/data

# Create non-root user
RUN groupadd -r appgroup && useradd -r -g appgroup appuser
RUN chown -R appuser:appgroup /app
USER appuser

# Set environment
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "Video-Project-Suite.Api.dll"]

################################################################################
# Default to production
FROM production AS final