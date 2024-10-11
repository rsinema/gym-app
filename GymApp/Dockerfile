# Use the official .NET SDK image as the build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# Set the working directory in the container
WORKDIR /src
# Copy the project file(s) and restore dependencies
COPY *.csproj ./
RUN dotnet restore
# Copy the rest of the source code
COPY . .
# Build and publish the app
RUN dotnet publish -c Release -o /app/publish/

# Use the official ASP.NET runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
# Set the working directory in the container
WORKDIR /app
# Copy the published app from the build stage
COPY --from=build /app/publish .
# expose the port
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80
# Start the app
ENTRYPOINT ["dotnet", "GymApp.dll"]