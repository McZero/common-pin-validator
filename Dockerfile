FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /app

COPY . .
# RUN dotnet build -o /app.
RUN dotnet restore
RUN dotnet publish -o /app/published-app -c release --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime
WORKDIR /app
COPY --from=build /app/published-app /app

RUN apk add --no-cache icu-libs krb5-libs libgcc libintl libssl3 libstdc++ zlib tzdata

ENV TZ=America/Mexico_City

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false


ENTRYPOINT [ "dotnet", "/app/HirCasa.CommonServices.PinValidator.API.dll" ]