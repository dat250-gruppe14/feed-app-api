#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FeedApp.Api/FeedApp.Api.csproj", "FeedApp.Api/"]
COPY ["FeedApp.Common/FeedApp.Common.csproj", "FeedApp.Common/"]
COPY ["FeedApp.Messaging/FeedApp.Messaging.csproj", "FeedApp.Messaging/"]
RUN dotnet restore "FeedApp.Api/FeedApp.Api.csproj"
COPY . .
WORKDIR "/src/FeedApp.Api"
RUN dotnet build "FeedApp.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FeedApp.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FeedApp.Api.dll"]
