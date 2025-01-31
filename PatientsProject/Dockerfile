# Use official .NET Core SDK image (Build Stage)
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /app

# Copy project files to restore dependencies properly
COPY ["PatientsProject.Api/PatientsProject.Api.csproj", "PatientsProject.Api/"]
COPY ["PatientsProject.Domain/PatientsProject.Domain.csproj", "PatientsProject.Domain/"]
COPY ["PatientsProject.Application/PatientsProject.Application.csproj", "PatientsProject.Application/"]
COPY ["PatientsProject.Infrastructure/PatientsProject.Infrastructure.csproj", "PatientsProject.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "./PatientsProject.Api/PatientsProject.Api.csproj"

# Copy everything
COPY . .

# Set working directory to API project
WORKDIR /app/PatientsProject.Api

# Build and publish
RUN dotnet publish -c Release -o /out

# Final runtime image (Optimized for smaller size)
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app

# Install curl
RUN apt-get update && apt-get install -y curl

# Copy published output from build stage
COPY --from=build /out .

# Start the API
ENTRYPOINT ["dotnet", "PatientsProject.Api.dll"]