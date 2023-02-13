FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["dotnet-aks-workload-identity/dotnet-aks-workload-identity.csproj", "dotnet-aks-workload-identity/"]
RUN dotnet restore "dotnet-aks-workload-identity/dotnet-aks-workload-identity.csproj"
COPY . .
WORKDIR "/src/dotnet-aks-workload-identity"
RUN dotnet build "dotnet-aks-workload-identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "dotnet-aks-workload-identity.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "dotnet-aks-workload-identity.dll"]
