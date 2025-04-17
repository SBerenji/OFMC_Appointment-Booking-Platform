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

# Install curl and supervisor
RUN apt-get update && \
    apt-get install -y curl supervisor && \
    apt-get clean && rm -rf /var/lib/apt/lists/*

# Copy published output
COPY --from=build /app/publish .

# Copy KeepAlive.sh script and Supervisord.config
COPY OFMC_Booking_Platform/OFMC_Booking_Platform/Scripts/KeepAlive.sh /app/Scripts/KeepAlive.sh
COPY OFMC_Booking_Platform/OFMC_Booking_Platform/Supervisord.conf /etc/Supervisord.conf

# Make keep-alive script executable
RUN chmod +x /app/Scripts/KeepAlive.sh

# Expose HTTP port (Render expects 8080)
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080 \
    ASPNETCORE_ENVIRONMENT=Development

# Start both the app and the keep-alive using supervisor
CMD ["supervisord", "-c", "/etc/Supervisord.conf"]