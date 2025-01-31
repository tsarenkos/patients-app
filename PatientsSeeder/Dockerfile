# Use .NET Core SDK image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copy project files
COPY ["PatientsSeeder/PatientsSeeder.csproj", "PatientsSeeder/"]
RUN dotnet restore "PatientsSeeder/PatientsSeeder.csproj"

# Copy everything and build
COPY ./PatientsSeeder ./PatientsSeeder
WORKDIR /app/PatientsSeeder
RUN dotnet publish -c Release -o /out

# Final runtime image
FROM mcr.microsoft.com/dotnet/runtime:6.0
WORKDIR /app
COPY --from=build /out .

ENTRYPOINT ["dotnet", "PatientsSeeder.dll"]