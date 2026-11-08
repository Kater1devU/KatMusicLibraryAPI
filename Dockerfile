
# Stage 1: Build application
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY KatMusicLibraryAPI/KatMusicLibraryAPI.csproj KatMusicLibraryAPI/
RUN dotnet restore KatMusicLibraryAPI/KatMusicLibraryAPI.csproj

COPY . .
WORKDIR /src/KatMusicLibraryAPI
RUN dotnet publish KatMusicLibraryAPI.csproj -c Release -o /app/publish

# Stage 2: Run application
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "KatMusicLibraryAPI.dll"]
