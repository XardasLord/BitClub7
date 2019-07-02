FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app

COPY . .

RUN dotnet restore ./BitClub7.sln

COPY . .

RUN dotnet publish ./BC7.Api/BC7.Api.csproj --output /app --configuration Release


#RUN dotnet restore "BC7.Api/BC7.Api.csproj"
#COPY . .
#WORKDIR "/src/BC7.Api"
#RUN dotnet build "BC7.Api.csproj" -c Release -o /app
#
#FROM build AS publish
#RUN dotnet publish "BC7.Api.csproj" -c Release -o /app


FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "BC7.Api.dll"]