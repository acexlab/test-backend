# Stage 1: Build and Publish the backend API
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ToDoApi.csproj", "./"]
RUN dotnet restore "ToDoApi.csproj"
COPY . .
RUN dotnet build "ToDoApi.csproj" -c Release -o /app/build
RUN dotnet publish "ToDoApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Create runtime container
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5140
ENV ASPNETCORE_URLS=http://+:5140
ENTRYPOINT ["dotnet", "ToDoApi.dll"]
