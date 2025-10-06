# ==============================
# STAGE 1: BUILD
# ==============================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj và restore riêng để cache tốt hơn
COPY ["CareerSpark.API/CareerSpark.API.csproj", "CareerSpark.API/"]
COPY ["CareerSpark.DataAccessLayer/CareerSpark.DataAccessLayer.csproj", "CareerSpark.DataAccessLayer/"]
COPY ["CareerSpark.BusinessLayer/CareerSpark.BusinessLayer.csproj", "CareerSpark.BusinessLayer/"]

RUN dotnet restore "CareerSpark.API/CareerSpark.API.csproj"

# Copy toàn bộ source code
COPY . .

# Build & publish
WORKDIR "/src/CareerSpark.API"
RUN dotnet publish "CareerSpark.API.csproj" -c Release -o /app/publish /p:UseAppHost=false


# ==============================
# STAGE 2: RUNTIME
# ==============================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy file đã publish từ stage build
COPY --from=build /app/publish .

# Add environment variable to ensure correct port in Render or Docker local
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Development


#  Expose both 8080 (Render) and 80 (local optional)
EXPOSE 8080
EXPOSE 80

#  Thêm healthcheck
HEALTHCHECK --interval=30s --timeout=5s --start-period=10s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "CareerSpark.API.dll"]
