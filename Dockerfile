#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base

WORKDIR /app

EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build

WORKDIR /src

COPY ["./StartUpProject.sln", "./"]
COPY ["./StartUpProject.App/StartUpProject.App.csproj", "StartUpProject.App/"]
COPY ["./HelloWorldProject/HelloWorldProject.csproj", "HelloWorldProject/"]

RUN dotnet restore "StartUpProject.App/StartUpProject.App.csproj"
RUN dotnet restore "HelloWorldProject/HelloWorldProject.csproj"

COPY . .

RUN dotnet build "/src/HelloWorldProject/HelloWorldProject.csproj" -c Release -o /app/build
RUN dotnet build "/src/StartUpProject.App/StartUpProject.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "/src/StartUpProject.App/StartUpProject.App.csproj" -c Release -o /app/publish
RUN dotnet publish "/src/HelloWorldProject/HelloWorldProject.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StartUpProject.App.dll", "HellowWorldProject.dll"]