FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Upload.Web.Sample/Upload.Web.Sample.csproj", "Upload.Web.Sample/"]
RUN dotnet restore "Upload.Web.Sample/Upload.Web.Sample.csproj"
COPY . .
WORKDIR "/src/Upload.Web.Sample"
RUN dotnet build "Upload.Web.Sample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Upload.Web.Sample.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Upload.Web.Sample.dll"]
