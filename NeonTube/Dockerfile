# NuGet restore
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY *.sln .
COPY NeonTube/*.csproj NeonTube/
RUN dotnet restore
COPY . .

# testing
FROM build AS testing
WORKDIR /src/NeonTube
RUN dotnet build

# publish
FROM build AS publish
WORKDIR /src/NeonTube
RUN dotnet publish -c Release -o /src/publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app
COPY --from=publish /src/publish .
# ENTRYPOINT ["dotnet", "NeonTube.dll"]
# heroku uses the following
CMD ASPNETCORE_URLS=http://*:$PORT dotnet NeonTube.dll