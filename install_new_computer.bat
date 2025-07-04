@echo off
echo ==========================================
echo   Configurador para Nueva Computadora
echo ==========================================
echo.

REM Verificar si estamos en la carpeta correcta
if not exist "SeleniumMcpServer" (
    echo ERROR: No se encontró la carpeta SeleniumMcpServer
    echo Asegúrate de ejecutar este script desde la carpeta raíz del proyecto
    pause
    exit /b 1
)

echo Verificando instalación de .NET...
dotnet --version >nul 2>&1
if errorlevel 1 (
    echo ERROR: .NET no está instalado
    echo.
    echo Pasos para instalar .NET:
    echo 1. Ir a: https://dotnet.microsoft.com/download/dotnet/9.0
    echo 2. Descargar .NET 9.0 SDK
    echo 3. Instalar y reiniciar el terminal
    echo 4. Ejecutar este script nuevamente
    pause
    exit /b 1
)

echo ✓ .NET está instalado (versión: 
dotnet --version
echo )

echo.
echo Verificando Chrome...
where chrome >nul 2>&1
if errorlevel 1 (
    echo ADVERTENCIA: Chrome no se encontró en el PATH
    echo El servidor intentará detectar Chrome automáticamente
    echo Si hay problemas, instala Google Chrome desde: https://www.google.com/chrome/
) else (
    echo ✓ Chrome encontrado
)

echo.
echo Limpiando proyecto...
dotnet clean --verbosity quiet >nul 2>&1

echo.
echo Compilando proyecto...
dotnet build --verbosity quiet
if errorlevel 1 (
    echo ERROR: Falló la compilación
    echo Mostrando errores detallados:
    dotnet build
    pause
    exit /b 1
)

echo ✓ Proyecto compilado exitosamente

echo.
echo Publicando ejecutable optimizado...
dotnet publish -c Release -o publish --self-contained false --verbosity quiet
if errorlevel 1 (
    echo ERROR: Falló la publicación
    echo Mostrando errores detallados:
    dotnet publish -c Release -o publish --self-contained false
    pause
    exit /b 1
)

echo ✓ Ejecutable publicado exitosamente

echo.
echo Configurando Claude Desktop...
set "PROJECT_PATH=%~dp0"
set "PROJECT_PATH=%PROJECT_PATH:~0,-1%"
set "CLAUDE_CONFIG=%APPDATA%\Claude\claude_desktop_config.json"

REM Crear directorio si no existe
if not exist "%APPDATA%\Claude" (
    mkdir "%APPDATA%\Claude"
)

REM Crear backup si el archivo existe
if exist "%CLAUDE_CONFIG%" (
    echo Creando backup de configuración existente...
    copy "%CLAUDE_CONFIG%" "%CLAUDE_CONFIG%.backup.%date:~-4,4%%date:~-10,2%%date:~-7,2%" >nul
)

REM Crear configuración optimizada
echo Escribiendo configuración optimizada...
(
echo {
echo   "mcpServers": {
echo     "selenium": {
echo       "command": "%PROJECT_PATH%\\publish\\SeleniumMcpServer.exe"
echo     }
echo   }
echo }
) > "%CLAUDE_CONFIG%"

echo ✓ Configuración de Claude Desktop actualizada

echo.
echo Probando servidor MCP...
echo (Esto debería mostrar que el servidor está funcionando)
timeout /t 3 /nobreak >nul
echo Iniciando prueba...
start /B "" "%PROJECT_PATH%\publish\SeleniumMcpServer.exe" >nul 2>&1
timeout /t 2 /nobreak >nul
tasklist /FI "IMAGENAME eq SeleniumMcpServer.exe" /FO CSV | find /I "SeleniumMcpServer.exe" >nul
if %ERRORLEVEL% EQU 0 (
    echo ✓ Servidor MCP funcionando correctamente
    taskkill /F /IM "SeleniumMcpServer.exe" /T >nul 2>&1
) else (
    echo ADVERTENCIA: No se pudo verificar el servidor MCP
    echo Prueba manual: ejecuta publish\SeleniumMcpServer.exe
)

echo.
echo ==========================================
echo   🎉 CONFIGURACIÓN COMPLETADA
echo ==========================================
echo.
echo ✅ PASOS COMPLETADOS:
echo   1. .NET verificado
echo   2. Proyecto compilado
echo   3. Ejecutable publicado
echo   4. Claude Desktop configurado
echo   5. Servidor MCP probado
echo.
echo 🚀 PRÓXIMOS PASOS:
echo   1. REINICIA Claude Desktop completamente
echo   2. Abre Claude Desktop
echo   3. Prueba comandos como: "start selenium browser"
echo   4. Verifica que aparezca "selenium" en la configuración MCP
echo.
echo 📁 ARCHIVOS CREADOS:
echo   • publish\SeleniumMcpServer.exe (ejecutable optimizado)
echo   • %CLAUDE_CONFIG% (configuración)
echo.
echo 🔧 SI HAY PROBLEMAS:
echo   • Revisa SOLUCION_PROBLEMAS.md
echo   • Ejecuta fix_and_build.bat
echo   • Verifica logs de Claude Desktop
echo.
echo 📋 INFORMACIÓN DEL SISTEMA:
echo   • Ruta del proyecto: %PROJECT_PATH%
echo   • Configuración Claude: %CLAUDE_CONFIG%
echo   • Ejecutable: %PROJECT_PATH%\publish\SeleniumMcpServer.exe
echo.

pause
