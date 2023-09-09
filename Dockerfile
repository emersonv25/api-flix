#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Api.MyFlix.csproj", "."]
RUN dotnet restore "./Api.MyFlix.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Api.MyFlix.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Api.MyFlix.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .


VOLUME /app/wwwroot/uploads


ENTRYPOINT ["dotnet", "Api.MyFlix.dll"]
