FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ScheduleSMK/ScheduleSMK.csproj", "ScheduleSMK/"]
RUN dotnet restore "ScheduleSMK/ScheduleSMK.csproj"
COPY . .
WORKDIR "/src/ScheduleSMK"
RUN dotnet build "ScheduleSMK.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ScheduleSMK.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ScheduleSMK.dll"]
