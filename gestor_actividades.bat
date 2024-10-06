
::Nos movemos a la ruta donde se encuentra el .csproj
cd ./API\PruebaBackendHackaton

:: Restaurar dependencias
dotnet restore

:: Compilación del proyecto
dotnet build --configuration Release

:: Ejecución del mismo
dotnet run --configuration Release

pause