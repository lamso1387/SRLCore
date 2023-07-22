FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine as build
WORKDIR /app
COPY . .
RUN dotnet restore 
RUN dotnet publish -c Release -o /app/deploy
FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine as runtime
WORKDIR /app
COPY --from=build /app/deploy /app
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
ENTRYPOINT [ "dotnet", "/app/barnameApi.dll" ]