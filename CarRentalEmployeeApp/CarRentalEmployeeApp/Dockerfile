FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY CarRentalEmployeeApp.csproj .
RUN dotnet restore CarRentalEmployeeApp.csproj

COPY . .
RUN dotnet build CarRentalEmployeeApp.csproj -c Release -o /app/build

RUN dotnet publish CarRentalEmployeeApp.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "CarRentalEmployeeApp.dll"]
