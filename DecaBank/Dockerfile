FROM mcr.microsoft.com/dotnet/sdk:5.0 AS base
WORKDIR /src
COPY *.sln .
COPY DecaBank.Tests/*.csproj DecaBank.Tests/
COPY DecaBank.Presentation/*.csproj DecaBank.Presentation/
COPY DecaBank.Data/*.csproj DecaBank.Data/
COPY DecaBank.Core/*.csproj DecaBank.Core/
COPY DecaBank.Model/*.csproj DecaBank.Model/

RUN dotnet restore

RUN curl --silent --location https://deb.nodesource.com/setup_10.x | bash -
RUN apt-get install --yes nodejs

COPY . .

#Testing
FROM base AS testing
WORKDIR /src/DecaBank.Presentation

RUN dotnet build
WORKDIR /src/DecaBank.Tests
RUN dotnet test

#Publishing
FROM base AS publish
WORKDIR /src/DecaBank.Presentation
RUN dotnet publish -c Release -o /src/publish

#Get the runtime into a folder called app
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app
COPY --from=publish /src/publish .
COPY --from=publish /src/DecaBank.Data/Data.Json/Customer.json .



#ENTRYPOINT ["dotnet", "AnimalFarmsMarket.Core.dll"]
CMD ASPNETCORE_URLS=http://*:$PORT dotnet DecaBank.Presentation.dll