# =================== Base Image ===================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Chạy container với biến môi trường cổng 8080
ENV ASPNETCORE_URLS=http://+:8080

# =================== Build Stage ===================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Sao chép file project để restore dependencies trước (cache build)
COPY ["GUIDME/GUIDME.csproj", "GUIDME/"]
COPY ["DataAccess/DataAccess.csproj", "DataAccess/"]
COPY ["Models/Models.csproj", "Models/"]
COPY ["Repositories/Repositories.csproj", "Repositories/"]

# Khôi phục dependencies (giúp cache hiệu quả hơn)
RUN dotnet restore "./GUIDME/GUIDME.csproj"

# Sao chép toàn bộ source code
COPY . .

# Build ứng dụng
WORKDIR "/src/GUIDME"
RUN dotnet build "./GUIDME.csproj" -c $BUILD_CONFIGURATION -o /app/build

# =================== Publish Stage ===================
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GUIDME.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# =================== Runtime Stage (Final) ===================
FROM base AS final
WORKDIR /app

# Sao chép từ build stage
COPY --from=publish /app/publish .

# Đặt quyền thực thi cho ứng dụng
RUN chmod +x /app/GUIDME.dll

# Kiểm tra xem container có đang chạy không
HEALTHCHECK --interval=30s --timeout=10s --start-period=10s --retries=3 \
  CMD curl --fail http://localhost:8080/health || exit 1

# Chạy ứng dụng
ENTRYPOINT ["dotnet", "GUIDME.dll"]
