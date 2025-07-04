@echo off
echo ==========================================
echo   Reparador MCP Selenium Server
echo ==========================================
echo.

echo Deteniendo procesos SeleniumMcpServer existentes...
tasklist /FI "IMAGENAME eq SeleniumMcpServer.exe" /FO CSV | find /I "SeleniumMcpServer.exe" >nul
if %ERRORLEVEL% EQU 0 (
    echo Terminando procesos SeleniumMcpServer...
    taskkill /F /IM "SeleniumMcpServer.exe" /T >nul 2>&1
    echo ✓ Procesos terminados
) else (
    echo ✓ No hay procesos SeleniumMcpServer ejecutándose
)

echo.
echo Esperando 2 segundos para que se liberen los archivos...
timeout /t 2 >nul

echo Limpiando proyecto...
cd /d "c:\Users\cesar\OneDrive\Documentos\ProyectosApex\MCPSeleniumServer"
dotnet clean --verbosity quiet >nul 2>&1

echo.
echo Publicando ejecutable optimizado...
REM Usar dotnet publish elimina completamente los mensajes de MSBuild
dotnet publish -c Release -o publish --self-contained false --verbosity quiet

if errorlevel 1 (
    echo ERROR: Falló la publicación
    echo.
    echo Intentando con más detalle...
    dotnet publish -c Release -o publish --self-contained false
    pause
    exit /b 1
) else (
    echo ✓ Ejecutable publicado exitosamente
)

echo.
echo Actualizando configuración de Claude Desktop...
set "CLAUDE_CONFIG=%APPDATA%\Claude\claude_desktop_config.json"

REM Crear backup del archivo existente si existe
if exist "%CLAUDE_CONFIG%" (
    copy "%CLAUDE_CONFIG%" "%CLAUDE_CONFIG%.backup" >nul
)

REM Crear configuración optimizada (usando ejecutable en lugar de dotnet run)
(
echo {
echo   "mcpServers": {
echo     "selenium": {
echo       "command": "C:\\Users\\cesar\\OneDrive\\Documentos\\ProyectosApex\\MCPSeleniumServer\\publish\\SeleniumMcpServer.exe"
echo     }
echo   }
echo }
) > "%CLAUDE_CONFIG%"

echo ✓ Configuración actualizada para usar ejecutable optimizado

echo.
echo ==========================================
echo   Reparación completada
echo ==========================================
echo.
echo CAMBIOS APLICADOS:
echo 1. ✓ Procesos terminados
echo 2. ✓ Proyecto limpiado
echo 3. ✓ Ejecutable publicado
echo 4. ✓ Configuración optimizada
echo.
echo PRÓXIMOS PASOS:
echo 1. Reinicia Claude Desktop completamente
echo 2. El servidor ahora usa ejecutable directo (sin MSBuild)
echo 3. No más errores de JSON
echo.
echo VENTAJAS:
echo • Inicio más rápido
echo • Sin mensajes de MSBuild
echo • Comunicación MCP limpia
echo.

pause
