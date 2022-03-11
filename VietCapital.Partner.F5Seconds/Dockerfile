FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal AS base
WORKDIR /app
EXPOSE 5000

ENV ASPNETCORE_URLS=http://+:5000

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
RUN apt update && apt upgrade -y

WORKDIR /src
COPY . .
WORKDIR "/src/VietCapital.Partner.F5Seconds.WebApp"
RUN dotnet restore
#RUN dotnet build -c Release -o /app/build
RUN dotnet publish -c Release -o /app/publish

#ReactJs build
#FROM node as nodebuilder

# Build Reactjs
# Install pre-reqs
RUN curl -fsSL https://deb.nodesource.com/setup_14.x | bash -
RUN apt install -y nodejs

WORKDIR "/src/VietCapital.Partner.F5Seconds.WebApp/ClientApp"
ARG REACT_APP_PREFIX_URL
ENV REACT_APP_PREFIX_URL=$REACT_APP_PREFIX_URL
RUN npm install && npm run build
#End ReactJs build

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
RUN mkdir -p /app/ClientApp/build
COPY --from=build /src/VietCapital.Partner.F5Seconds.WebApp/ClientApp/build/. /app/ClientApp/build/
COPY --from=build /src/VietCapital.Partner.F5Seconds.WebApp/ClientApp/.env /app/ClientApp/build/.env
ENTRYPOINT ["dotnet", "VietCapital.Partner.F5Seconds.WebApp.dll"]
