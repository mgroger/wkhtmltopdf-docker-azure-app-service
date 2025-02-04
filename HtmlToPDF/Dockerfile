# See https://github.com/madnight/docker-alpine-wkhtmltopdf/tree/master, https://github.com/HakanL/WkHtmlToPdf-DotNet/issues/89

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
RUN apk update
RUN apk add bash icu-libs krb5-libs libgcc libintl libstdc++ zlib
RUN apk add libgdiplus --no-cache --repository http://dl-cdn.alpinelinux.org/alpine/v3.17/community/ --allow-untrusted

# ln: /lib/x86_64-linux-gnu/libgdiplus.so: No such file or directory
# RUN ln -s /usr/lib/libgdiplus.so /lib/x86_64-linux-gnu/libgdiplus.so

RUN wget gdebi
RUN wget https://github.com/wkhtmltopdf/wkhtmltopdf/releases/download/0.12.5/wkhtmltox_0.12.5-1.stretch_amd64.deb
RUN gdebi --n wkhtmltox_0.12.5-1.stretch_amd64.deb
RUN apk add libssl1.1
RUN ln -s /usr/local/lib/libwkhtmltox.so /usr/lib/libwkhtmltox.so

WORKDIR /app
EXPOSE 80

# Install dependencies
RUN apk update && \
    apk add --no-cache \
        bash \
        icu-libs \
        krb5-libs \
        libgcc \
        libintl \
        libssl1.1 \
        libstdc++ \
        zlib \
        libstdc++ \
        libx11 \
        libxrender \
        libxext \
        libssl1.1 \
        libjpeg \
        gcompat \
        ca-certificates \
        fontconfig \
        freetype \
        ttf-dejavu \
        ttf-droid \
        ttf-freefont \
        ttf-liberation && \
    ln -s /usr/lib/libjpeg.so.8 /usr/lib/libjpeg.so.62

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