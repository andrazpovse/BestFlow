FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /


COPY BestFlow ./src
RUN cd src && dotnet publish "BestFlow.csproj" -c Release -o /app

# Cleanup image
RUN rm -r /src

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app

# Copy from build
COPY --from=build /app .

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "BestFlow.dll"]
