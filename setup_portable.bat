@echo off
echo ==========================================
echo   Configurador MCP Selenium Server
echo ==========================================
echo.

REM Obtener la ruta del directorio actual
set "PROJECT_PATH=%~dp0"
set "PROJECT_PATH=%PROJECT_PATH:~0,-1%"

echo Ruta del proyecto: %PROJECT_PATH%
echo.

REM Ruta del archivo de configuración de Claude Desktop
set "CLAUDE_CONFIG=%APPDATA%\Claude\claude_desktop_config.json"

echo Verificando instalación de .NET...
dotnet --version >nul 2>&1
if errorlevel 1 (
    echo ERROR: .NET no está instalado o no está en el PATH
    echo Descarga .NET desde: https://dotnet.microsoft.com/download
    pause
    exit /b 1
)

echo ✓ .NET está instalado
echo.

echo Verificando Chrome...
where chrome >nul 2>&1
if errorlevel 1 (
    echo ADVERTENCIA: Chrome no se encontró en el PATH
    echo El servidor intentará encontrar Chrome automáticamente
) else (
    echo ✓ Chrome encontrado
)
echo.

echo Construyendo el proyecto...
echo (Forzando idioma inglés para evitar problemas de JSON)
set LC_ALL=en_US.UTF-8
set LANG=en_US.UTF-8
dotnet build --verbosity quiet
if errorlevel 1 (
    echo ERROR: Falló la compilación del proyecto
    echo Intentando limpieza y nueva compilación...
    dotnet clean
    dotnet build --verbosity quiet
    if errorlevel 1 (
        echo ERROR: Falló la compilación después de limpieza
        pause
        exit /b 1
    )
)

echo ✓ Proyecto compilado exitosamente
echo.

echo Configurando Claude Desktop...

REM Crear backup del archivo existente si existe
if exist "%CLAUDE_CONFIG%" (
    echo Creando backup de la configuración existente...
    copy "%CLAUDE_CONFIG%" "%CLAUDE_CONFIG%.backup" >nul
    echo ✓ Backup creado: %CLAUDE_CONFIG%.backup
)

REM Crear el contenido de la configuración
echo Escribiendo nueva configuración...
echo.
echo Selecciona el modo de configuración:
echo 1. Ejecutable optimizado (MÁS RÁPIDO, sin problemas JSON)
echo 2. Dotnet run (más portable, pero puede tener problemas JSON)
echo.
set /p "choice=Ingresa tu opción (1 o 2): "

if "%choice%"=="1" (
    echo Configurando modo ejecutable optimizado...
    dotnet publish -c Release -o publish --self-contained false --verbosity quiet
    if errorlevel 1 (
        echo ERROR: Falló la publicación
        pause
        exit /b 1
    )
    (
    echo {
    echo   "mcpServers": {
    echo     "selenium": {
    echo       "command": "%PROJECT_PATH%\\publish\\SeleniumMcpServer.exe"
    echo     }
    echo   }
    echo }
    ) > "%CLAUDE_CONFIG%"
    echo ✓ Configuración optimizada aplicada
) else (
    echo Configurando modo dotnet run...
    (
    echo {
    echo   "mcpServers": {
    echo     "selenium": {
    echo       "command": "dotnet",
    echo       "args": ["run", "--project", "SeleniumMcpServer", "--verbosity", "quiet"],
    echo       "cwd": "%PROJECT_PATH%",
    echo       "env": {
    echo         "LC_ALL": "en_US.UTF-8",
    echo         "LANG": "en_US.UTF-8",
    echo         "DOTNET_CLI_UI_LANGUAGE": "en"
    echo       }
    echo     }
    echo   }
    echo }
    ) > "%CLAUDE_CONFIG%"
    echo ✓ Configuración portable aplicada
)

echo ✓ Configuración actualizada
echo.

echo ==========================================
echo   Configuración completada
echo ==========================================
echo.
echo Configuración creada en: %CLAUDE_CONFIG%
echo.
echo PRÓXIMOS PASOS:
echo 1. Reinicia Claude Desktop
echo 2. Verifica que el servidor MCP aparezca en la configuración
echo 3. Prueba usando los comandos Selenium en Claude
echo.
echo Para usar en otro equipo:
echo 1. Copia toda la carpeta del proyecto
echo 2. Ejecuta este script en el nuevo equipo
echo 3. Reinicia Claude Desktop
echo.

pause
