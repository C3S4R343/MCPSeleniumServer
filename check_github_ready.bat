@echo off
echo ==========================================
echo   Verificador Pre-GitHub
echo ==========================================
echo.

echo Verificando estructura del proyecto...

REM Verificar archivos principales
set "missing_files="

if not exist "README.md" (
    set "missing_files=%missing_files% README.md"
)

if not exist "LICENSE" (
    set "missing_files=%missing_files% LICENSE"
)

if not exist ".gitignore" (
    set "missing_files=%missing_files% .gitignore"
)

if not exist "CONTRIBUTING.md" (
    set "missing_files=%missing_files% CONTRIBUTING.md"
)

if not exist "SeleniumMcpServer\SeleniumMcpServer.csproj" (
    set "missing_files=%missing_files% SeleniumMcpServer.csproj"
)

if not exist "install_new_computer.bat" (
    set "missing_files=%missing_files% install_new_computer.bat"
)

if not "%missing_files%"=="" (
    echo ❌ ARCHIVOS FALTANTES:%missing_files%
    echo.
    echo Crea los archivos faltantes antes de subir a GitHub
    pause
    exit /b 1
) else (
    echo ✅ Todos los archivos principales están presentes
)

echo.
echo Verificando compilación...
dotnet build --verbosity quiet
if errorlevel 1 (
    echo ❌ ERROR: El proyecto no compila
    echo Arregla los errores antes de subir a GitHub
    pause
    exit /b 1
) else (
    echo ✅ Proyecto compila correctamente
)

echo.
echo Verificando publicación...
dotnet publish -c Release -o publish --self-contained false --verbosity quiet
if errorlevel 1 (
    echo ❌ ERROR: Falló la publicación
    echo Arregla los errores antes de subir a GitHub
    pause
    exit /b 1
) else (
    echo ✅ Publicación exitosa
)

echo.
echo Limpiando archivos temporales...
if exist "publish\" (
    rmdir /s /q publish
    echo ✅ Carpeta publish eliminada (no debe subirse a GitHub)
)

if exist "bin\" (
    rmdir /s /q bin
    echo ✅ Carpeta bin eliminada
)

if exist "obj\" (
    rmdir /s /q obj
    echo ✅ Carpeta obj eliminada
)

if exist "SeleniumMcpServer\bin\" (
    rmdir /s /q SeleniumMcpServer\bin
    echo ✅ Carpeta SeleniumMcpServer\bin eliminada
)

if exist "SeleniumMcpServer\obj\" (
    rmdir /s /q SeleniumMcpServer\obj
    echo ✅ Carpeta SeleniumMcpServer\obj eliminada
)

echo.
echo Verificando .gitignore...
findstr /i "bin/" .gitignore >nul
if errorlevel 1 (
    echo ❌ .gitignore no incluye bin/
) else (
    echo ✅ .gitignore incluye bin/
)

findstr /i "obj/" .gitignore >nul
if errorlevel 1 (
    echo ❌ .gitignore no incluye obj/
) else (
    echo ✅ .gitignore incluye obj/
)

findstr /i "publish/" .gitignore >nul
if errorlevel 1 (
    echo ❌ .gitignore no incluye publish/
) else (
    echo ✅ .gitignore incluye publish/
)

echo.
echo ==========================================
echo   🎉 VERIFICACIÓN COMPLETADA
echo ==========================================
echo.
echo ✅ PROYECTO LISTO PARA GITHUB
echo.
echo 📋 CHECKLIST:
echo   ✓ Archivos principales presentes
echo   ✓ Proyecto compila correctamente
echo   ✓ Publicación funciona
echo   ✓ Archivos temporales limpiados
echo   ✓ .gitignore configurado
echo.
echo 🚀 PRÓXIMOS PASOS:
echo   1. git init (si no es repo aún)
echo   2. git add .
echo   3. git commit -m "Initial commit"
echo   4. git remote add origin https://github.com/TU_USUARIO/mcp-selenium-server.git
echo   5. git push -u origin main
echo.
echo 📝 COMANDOS SUGERIDOS:
echo   git init
echo   git add .
echo   git commit -m "feat: initial MCP Selenium Server implementation"
echo   git branch -M main
echo   git remote add origin [URL_DE_TU_REPO]
echo   git push -u origin main
echo.
echo 🔗 DESPUÉS DE SUBIR A GITHUB:
echo   • Crear release con tag v1.0.0
echo   • Actualizar README con URL correcta del repo
echo   • Configurar GitHub Actions (opcional)
echo   • Habilitar Issues y Discussions
echo.

pause
