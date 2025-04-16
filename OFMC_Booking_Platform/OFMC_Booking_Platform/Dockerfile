# This stage is used to build the service project
# ----- Build Stage -----
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /app
COPY ["OFMC_Booking_Platform/OFMC_Booking_Platform.csproj", "OFMC_Booking_Platform/"]
RUN dotnet restore "./OFMC_Booking_Platform/OFMC_Booking_Platform.csproj"
COPY . .
WORKDIR "/app/OFMC_Booking_Platform"
RUN dotnet build "./OFMC_Booking_Platform.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./OFMC_Booking_Platform.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OFMC_Booking_Platform.dll"]