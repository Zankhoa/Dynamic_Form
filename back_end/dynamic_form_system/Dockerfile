FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["dynamic_form_system/dynamic_form_system.csproj", "dynamic_form_system/"]
RUN dotnet restore "dynamic_form_system/dynamic_form_system.csproj"

COPY . .
WORKDIR "/src/dynamic_form_system"
RUN dotnet build "dynamic_form_system.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "dynamic_form_system.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "dynamic_form_system.dll"]