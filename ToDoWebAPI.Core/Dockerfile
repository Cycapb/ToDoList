FROM mcr.microsoft.com/dotnet/aspnet:5.0-bullseye-slim as base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-bullseye-slim as build
WORKDIR /src
COPY ["ToDoBussinessLogic/ToDoBussinessLogic.csproj","ToDoBussinessLogic/"]
COPY ["ToDoDAL.Core/ToDoDAL.Core.csproj","ToDoDAL.Core/"]
COPY ["ToDoProviders/ToDoProviders.csproj","ToDoProviders/"]
COPY ["ToDoWebAPI.Core/ToDoWebAPI.Core.csproj","ToDoWebAPI.Core/"]
COPY ["ToDoDomainModels/ToDoDomainModels.csproj","ToDoDomainModels/"]
RUN dotnet restore "ToDoWebAPI.Core/ToDoWebAPI.Core.csproj"
COPY . .
WORKDIR "/src/ToDoWebAPI.Core"
RUN dotnet build "ToDoWebAPI.Core.csproj" -c Debug -o /app/build

FROM build as publish
RUN dotnet publish "ToDoWebAPI.Core.csproj" -c Debug -o /app/publish

FROM base as final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "ToDoWebAPI.Core.dll" ]
