# ==============================
# STAGE 1: BUILD
# ==============================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj và restore để cache hiệu quả
COPY ["CareerSpark.API/CareerSpark.API.csproj", "CareerSpark.API/"]
COPY ["CareerSpark.DataAccessLayer/CareerSpark.DataAccessLayer.csproj", "CareerSpark.DataAccessLayer/"]
COPY ["CareerSpark.BusinessLayer/CareerSpark.BusinessLayer.csproj", "CareerSpark.BusinessLayer/"]
RUN dotnet restore "CareerSpark.API/CareerSpark.API.csproj"

# Copy toàn bộ source code và publish
COPY . .
WORKDIR "/src/CareerSpark.API"
RUN dotnet publish "CareerSpark.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ==============================
# STAGE 2: RUNTIME
# ==============================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Cài curl (để healthcheck không lỗi)
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Copy output đã publish
COPY --from=build /app/publish .

# Environment
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Expose cổng thật sẽ dùng
EXPOSE 8080

# Healthcheck
HEALTHCHECK --interval=30s --timeout=5s --start-period=10s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "CareerSpark.API.dll"]
