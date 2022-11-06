FROM mcr.microsoft.com/dotnet/aspnet:6.0 as base
WORKDIR /app
EXPOSE 5282
EXPOSE 7282

FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /src

COPY *.sln .
COPY Papyrus/*.csproj ./Papyrus/
COPY Papyrus.Logic/*.csproj ./Papyrus.Logic/
COPY Papyrus.Mongo.DataAccess/*.csproj ./Papyrus.Mongo.DataAccess/
COPY Papyrus.DataAccess/*.csproj ./Papyrus.DataAccess/
COPY Papyrus.Client/*.csproj ./Papyrus.Client/
COPY Papyrus.Shared/*.csproj ./Papyrus.Shared/

RUN dotnet restore

COPY Papyrus/. ./Papyrus/
COPY Papyrus.Logic/. ./Papyrus.Logic/
COPY Papyrus.Mongo.DataAccess/. ./Papyrus.Mongo.DataAccess/
COPY Papyrus.DataAccess/. ./Papyrus.DataAccess/
COPY Papyrus.Shared/. ./Papyrus.Shared/

WORKDIR /src/Papyrus

RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Papyrus.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Papyrus.dll"]
