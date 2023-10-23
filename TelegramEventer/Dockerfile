FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TelegramEventer/TelegramEventer.csproj", "TelegramEventer/"]
RUN dotnet restore "TelegramEventer/TelegramEventer.csproj"
COPY . .
WORKDIR "/src/TelegramEventer"
RUN dotnet build "TelegramEventer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TelegramEventer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TelegramEventer.dll"]
