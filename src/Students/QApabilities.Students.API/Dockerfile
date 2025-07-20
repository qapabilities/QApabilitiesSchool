# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["QApabilities.Students.API/QApabilities.Students.API.csproj", "QApabilities.Students.API/"]
COPY ["../Shared/QApabilities.Shared/QApabilities.Shared.csproj", "../Shared/QApabilities.Shared/"]
RUN dotnet restore "QApabilities.Students.API/QApabilities.Students.API.csproj"

# Copy source code
COPY . .

# Build the application
WORKDIR "/src/QApabilities.Students.API"
RUN dotnet build "QApabilities.Students.API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "QApabilities.Students.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Create non-root user
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

# Copy published application
COPY --from=publish --chown=appuser:appuser /app/publish .

# Expose port
EXPOSE 5001

# Health check
HEALTHCHECK --interval=30s --timeout=3s --retries=3 \
    CMD curl -f http://localhost:5001/health || exit 1

# Start the application
ENTRYPOINT ["dotnet", "QApabilities.Students.API.dll"] 