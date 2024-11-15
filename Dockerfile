# Use the official .NET SDK image as the base image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy the solution file and restore dependencies
COPY *.sln ./
COPY GymApp.Web/*.csproj ./GymApp.Web/
COPY GymApp.Models/*.csproj ./GymApp.Models/
COPY GymApp.Services/*.csproj ./GymApp.Services/
COPY GymApp.Repositories/*.csproj ./GymApp.Repositories/
RUN dotnet restore

# Copy the rest of the code
COPY . ./

# Build and publish the application
RUN dotnet publish -c Release -o out

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "GymApp.Web.dll"]