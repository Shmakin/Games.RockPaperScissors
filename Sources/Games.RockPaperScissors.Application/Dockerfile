FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Sources/Games.RockPaperScissors.Application/Games.RockPaperScissors.Application.csproj", "Games.RockPaperScissors.Application/"]
RUN dotnet restore "Sources/Games.RockPaperScissors.Application/Games.RockPaperScissors.Application.csproj"
COPY . .
WORKDIR "/src/Games.RockPaperScissors.Application"
RUN dotnet build "Games.RockPaperScissors.Application.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Games.RockPaperScissors.Application.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Games.RockPaperScissors.Application.dll"]
