# Sử dụng image có .NET SDK để build ứng dụng API và migration database
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copy mã nguồn và restore các dependency
COPY *.csproj .
RUN dotnet restore

# Copy toàn bộ mã nguồn và build ứng dụng
COPY . .
RUN dotnet publish -c Release -o out

# Thêm công cụ Entity Framework Core CLI vào image
RUN dotnet tool install --global dotnet-ef --version 6.0.19

# Thêm đường dẫn của công cụ EF vào biến PATH
ENV PATH="$PATH:/root/.dotnet/tools"

# RUN dotnet ef database update

# Sử dụng image có .NET runtime để chạy ứng dụng
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out .

# Chạy ứng dụng API
ENTRYPOINT ["dotnet", "server.dll"]