# 🚀 Guía de Instalación para Nueva Computadora

## 📋 Requisitos Previos

Antes de empezar, asegúrate de tener:
- **Windows 10/11** (o Linux/macOS)
- **Conexión a internet** para descargar dependencias
- **Claude Desktop** instalado

## 🔧 Instalación Automática (Windows)

### 1. Descargar .NET 9.0 SDK
```powershell
# Ir a: https://dotnet.microsoft.com/download/dotnet/9.0
# Descargar e instalar .NET 9.0 SDK
```

### 2. Clonar el repositorio
```bash
git clone https://github.com/TU_USUARIO/mcp-selenium-server.git
cd mcp-selenium-server
```

### 3. Ejecutar configuración automática
```bash
# Doble clic en setup_portable.bat
# O desde PowerShell:
.\setup_portable.bat
```

### 4. Reiniciar Claude Desktop
- Cierra Claude Desktop completamente
- Vuelve a abrir Claude Desktop
- Verifica que aparezca "selenium" en la configuración MCP

## 🛠️ Instalación Manual

### Paso 1: Verificar .NET
```powershell
# Verificar instalación de .NET
dotnet --version
# Debe mostrar: 9.0.x o superior
```

### Paso 2: Instalar Chrome (si no está instalado)
```powershell
# Descargar desde: https://www.google.com/chrome/
# O verificar si está instalado:
where chrome
```

### Paso 3: Compilar proyecto
```powershell
# Limpiar proyecto
dotnet clean

# Compilar
dotnet build

# Publicar ejecutable optimizado
dotnet publish -c Release -o publish --self-contained false
```

### Paso 4: Configurar Claude Desktop
Editar archivo: `%APPDATA%\Claude\claude_desktop_config.json`

```json
{
  "mcpServers": {
    "selenium": {
      "command": "RUTA_COMPLETA_DEL_PROYECTO\\publish\\SeleniumMcpServer.exe"
    }
  }
}
```

**Ejemplo de ruta completa:**
```json
{
  "mcpServers": {
    "selenium": {
      "command": "C:\\Users\\USUARIO\\Projects\\mcp-selenium-server\\publish\\SeleniumMcpServer.exe"
    }
  }
}
```

## 🔄 Configuración Portable (Para múltiples equipos)

### Opción 1: Variables de entorno
```powershell
# Crear variable de entorno
[Environment]::SetEnvironmentVariable("MCP_SELENIUM_PATH", "C:\TuProyecto\mcp-selenium-server", "User")
```

Configuración Claude Desktop:
```json
{
  "mcpServers": {
    "selenium": {
      "command": "dotnet",
      "args": ["run", "--project", "SeleniumMcpServer", "--verbosity", "quiet"],
      "cwd": "%MCP_SELENIUM_PATH%",
      "env": {
        "DOTNET_CLI_UI_LANGUAGE": "en"
      }
    }
  }
}
```

### Opción 2: Script de configuración
Crear `configure_new_machine.bat`:
```batch
@echo off
set "PROJECT_PATH=%~dp0"
set "CLAUDE_CONFIG=%APPDATA%\Claude\claude_desktop_config.json"

dotnet publish -c Release -o publish --self-contained false

(
echo {
echo   "mcpServers": {
echo     "selenium": {
echo       "command": "%PROJECT_PATH%publish\\SeleniumMcpServer.exe"
echo     }
echo   }
echo }
) > "%CLAUDE_CONFIG%"

echo Configuración completada. Reinicia Claude Desktop.
pause
```

## 🧪 Verificación de Instalación

### 1. Probar compilación
```powershell
dotnet build
# Debe completarse sin errores
```

### 2. Probar ejecución
```powershell
.\publish\SeleniumMcpServer.exe
# Debe mostrar: "Server (stream) (SeleniumMcpServer) transport reading messages"
# Presionar Ctrl+C para salir
```

### 3. Probar Claude Desktop
1. Reiniciar Claude Desktop
2. En logs debe aparecer: "Server started and connected successfully"
3. Probar comando: "start selenium browser"

## 🐛 Solución de Problemas Comunes

### Error: "dotnet no se reconoce"
```powershell
# Reinstalar .NET SDK desde:
# https://dotnet.microsoft.com/download/dotnet/9.0
```

### Error: "Chrome no encontrado"
```powershell
# Instalar Google Chrome
# O definir ruta manualmente:
set CHROME_BIN=C:\Program Files\Google\Chrome\Application\chrome.exe
```

### Error: "Access denied"
```powershell
# Ejecutar como administrador
# O cambiar permisos de la carpeta
```

### Error: "JSON parse error"
```powershell
# Usar ejecutable en lugar de dotnet run
# Ver SOLUCION_PROBLEMAS.md para más detalles
```

## 📋 Checklist de Instalación

- [ ] .NET 9.0 SDK instalado
- [ ] Google Chrome instalado
- [ ] Proyecto clonado
- [ ] Proyecto compilado exitosamente
- [ ] Ejecutable publicado
- [ ] Claude Desktop configurado
- [ ] Servidor MCP funcionando
- [ ] Prueba exitosa con Claude

## 🔧 Configuraciones Avanzadas

### Para desarrollo
```json
{
  "mcpServers": {
    "selenium": {
      "command": "dotnet",
      "args": ["run", "--project", "SeleniumMcpServer"],
      "cwd": "RUTA_DEL_PROYECTO"
    }
  }
}
```

### Para producción
```json
{
  "mcpServers": {
    "selenium": {
      "command": "RUTA_COMPLETA\\publish\\SeleniumMcpServer.exe"
    }
  }
}
```

## 🌐 Instalación en Linux/macOS

```bash
# Instalar .NET
curl -sSL https://dot.net/v1/dotnet-install.sh | bash

# Compilar proyecto
dotnet publish -c Release -o publish

# Configurar Claude Desktop (ajustar ruta según OS)
# Linux: ~/.config/Claude/claude_desktop_config.json
# macOS: ~/Library/Application Support/Claude/claude_desktop_config.json
```

## 🚀 ¡Listo para usar!

Una vez completada la instalación, puedes usar comandos como:
- "Start a Chrome browser"
- "Navigate to https://example.com"
- "Close the browser session"

¡Disfruta automatizando con MCP Selenium Server! 🎉
