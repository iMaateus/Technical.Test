FROM public.ecr.aws/lambda/dotnet:5.0 AS base

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build
WORKDIR /src
COPY ["Technical.Test.DeleteBinary.csproj", "Technical.Test.DeleteBinary/"]
RUN dotnet restore "Technical.Test.DeleteBinary/Technical.Test.DeleteBinary.csproj"

WORKDIR "/src/Technical.Test.DeleteBinary"
COPY . .
RUN dotnet build "Technical.Test.DeleteBinary.csproj" --configuration Release --output /app/build

FROM build AS publish
RUN dotnet publish "Technical.Test.DeleteBinary.csproj" \
            --configuration Release \ 
            --runtime linux-x64 \
            --self-contained false \ 
            --output /app/publish \
            -p:PublishReadyToRun=true  

FROM base AS final
WORKDIR /var/task
COPY --from=publish /app/publish .

CMD [ "Technical.Test.DeleteBinary::Technical.Test.DeleteBinary.Function::FunctionHandler"]