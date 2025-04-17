# ----- Build Stage -----
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY ["OFMC_Booking_Platform/OFMC_Booking_Platform/OFMC_Booking_Platform.csproj", "OFMC_Booking_Platform/"]
RUN dotnet restore "OFMC_Booking_Platform/OFMC_Booking_Platform.csproj"
COPY . .
WORKDIR "/src/OFMC_Booking_Platform"
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ----- Runtime Stage -----
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Copy published output
COPY --from=build /app/publish .

# Expose HTTP port (Render expects 8080)
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080 \
    ASPNETCORE_ENVIRONMENT=Development

# Entry point
ENTRYPOINT ["dotnet", "OFMC_Booking_Platform.dll"]