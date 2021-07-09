FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app

#ENV ASPNETCORE_ENVIRONMENT "Development"
ENV ASPNETCORE_ENVIRONMENT "Production"

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

COPY ["src/Technical.Test.Api/Technical.Test.Api.csproj", "src/Technical.Test.Api/"]
COPY ["src/Technical.Test.Business/Technical.Test.Business.csproj", "src/Technical.Test.Business/"]
COPY ["src/Technical.Test.Data/Technical.Test.Data.csproj", "src/Technical.Test.Data/"]

RUN dotnet restore "src/Technical.Test.Api/Technical.Test.Api.csproj"
COPY . .
WORKDIR "src/Technical.Test.Api/"
RUN dotnet build "Technical.Test.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Technical.Test.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

CMD ASPNETCORE_URLS="http://*:$PORT" dotnet Technical.Test.Api.dll