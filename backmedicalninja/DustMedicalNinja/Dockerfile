FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
RUN mkdir -p /images/signature
EXPOSE 8443
ENV ASPNETCORE_URLS=http://+:8443


#ADD timezone to carraska
ENV TZ=Brazil/East
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY DustMedicalNinja/DustMedicalNinja.csproj DustMedicalNinja/
RUN dotnet restore DustMedicalNinja/DustMedicalNinja.csproj
COPY . .
WORKDIR /src/DustMedicalNinja

RUN dotnet build DustMedicalNinja.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish DustMedicalNinja.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DustMedicalNinja.dll"]
