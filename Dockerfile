# Use the SDK image to build the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Use the official image as a parent image for the runtime
FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=build /app/out ./
EXPOSE 80
ENTRYPOINT ["dotnet", "SplitIt.dll"]
