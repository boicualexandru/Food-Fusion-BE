FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
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

EXPOSE 5000/tcp
ENV ASPNETCORE_URLS http://*:5000
ENV ASPNETCORE_ENVIRONMENT docker
ENV ConnectionStrings__FoodFusionDB docker

ENTRYPOINT ["dotnet", "WebApi.dll"]
