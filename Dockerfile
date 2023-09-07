# Use an official Windows-based .NET SDK image as a parent image
FROM mcr.microsoft.com/dotnet/sdk:6.0-windowsservercore-ltsc2022 AS build-env

# Set the working directory in the container
WORKDIR /app

# Copy the project file and restore any dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the application source code
COPY . ./

# Build the application
RUN dotnet publish -c Release -o out

# Build the final image
FROM mcr.microsoft.com/dotnet/runtime:6.0-windowsservercore-ltsc2022
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["findhash.exe"]
