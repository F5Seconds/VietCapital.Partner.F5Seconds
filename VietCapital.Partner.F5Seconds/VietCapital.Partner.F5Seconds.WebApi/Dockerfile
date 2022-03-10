FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal AS base
WORKDIR /app
EXPOSE 5055

ENV ASPNETCORE_URLS=http://+:5055

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /src
COPY ["VietCapital.Partner.F5Seconds/VietCapital.Partner.F5Seconds.WebApi/VietCapital.Partner.F5Seconds.WebApi.csproj", "/"]
COPY . .
WORKDIR "/src"

FROM build AS publish
RUN dotnet publish "VietCapital.Partner.F5Seconds/VietCapital.Partner.F5Seconds.WebApi/VietCapital.Partner.F5Seconds.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ADD VietCapital.Partner.F5Seconds/VietCapital.Partner.F5Seconds.WebApi/VietCapital.Partner.F5Seconds.WebApi.xml /app/
ENTRYPOINT ["dotnet", "VietCapital.Partner.F5Seconds.WebApi.dll"]
