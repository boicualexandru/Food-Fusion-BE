FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["FoodFusion/WebApi.csproj", "FoodFusion/"]
COPY ["DataAccess/DataAccess.csproj", "DataAccess/"]
COPY ["Services/Services.csproj", "Services/"]
COPY ["Common/Common.csproj", "Common/"]
RUN dotnet restore "FoodFusion/WebApi.csproj"
COPY . .
WORKDIR "/src/FoodFusion"
RUN dotnet build "WebApi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "WebApi.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WebApi.dll"]

ENV ASPNETCORE_URLS=http://+:5000