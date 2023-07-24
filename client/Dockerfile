# Sử dụng image có .NET SDK để build ứng dụng Razor Pages
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copy mã nguồn và restore các dependency
COPY *.csproj .
RUN dotnet restore

# Copy toàn bộ mã nguồn và build ứng dụng
COPY . .
RUN dotnet publish -c Release -o out

# Sử dụng image có .NET runtime để chạy ứng dụng
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out .

# Chạy ứng dụng Razor Pages
ENTRYPOINT ["dotnet", "client.dll"]