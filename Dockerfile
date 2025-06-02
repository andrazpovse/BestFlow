FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

# Copy projects
COPY BestFlow.Api/BestFlow.Api.csproj BestFlow.Api/
COPY BestFlow.Library/BestFlow.Library.csproj BestFlow.Library/

# Restore
RUN dotnet restore BestFlow.Api/BestFlow.Api.csproj

# Copy source code
COPY BestFlow.Api/ BestFlow.Api/
COPY BestFlow.Library/ BestFlow.Library/

# Publish
RUN dotnet publish BestFlow.Api/BestFlow.Api.csproj -c Release -o /app

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app

COPY --from=build /app .

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "BestFlow.Api.dll"]
