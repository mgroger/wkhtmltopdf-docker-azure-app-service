# See https://github.com/madnight/docker-alpine-wkhtmltopdf/tree/master, https://github.com/HakanL/WkHtmlToPdf-DotNet/issues/89

FROM alpine:3.13
MAINTAINER Fabian Beuke <mail@beuke.org>
RUN apk add --update --no-cache \
    libgcc libstdc++ libx11 glib libxrender libxext libintl \
    ttf-dejavu ttf-droid ttf-freefont ttf-liberation 

# On alpine static compiled patched qt headless wkhtmltopdf (46.8 MB).
COPY --from=madnight/alpine-wkhtmltopdf-builder:0.12.5-alpine3.10-606718795 \
    /bin/wkhtmltopdf /bin/wkhtmltopdf
RUN [ "$(sha256sum /bin/wkhtmltopdf | awk '{ print $1 }')" \
        == "06139f13500db9b0b4373d40ff0faf046e536695fa836e92f41d829696d6859f" ]

WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY HtmlToPDF.csproj HtmlToPDF/
RUN dotnet restore HtmlToPDF/HtmlToPDF.csproj
COPY . ./HtmlToPDF/
WORKDIR /src/HtmlToPDF
RUN dotnet build HtmlToPDF.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish HtmlToPDF.csproj -c Release -o /app

FROM base AS final
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "HtmlToPDF.dll"]