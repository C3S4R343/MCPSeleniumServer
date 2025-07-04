# MCP Selenium Server

Un servidor Model Context Protocol (MCP) para automatización web con Selenium en C#. Permite controlar navegadores web desde Claude Desktop y otros clientes MCP.

## 🚀 Características

- **Control de navegadores**: Abre y controla Chrome/Firefox
- **Navegación web**: Navega a cualquier URL
- **Gestión de sesiones**: Maneja múltiples sesiones de navegador
- **Ventana visible**: Por defecto muestra la ventana del navegador para ver las pruebas
- **Logs limpios**: Suprime mensajes innecesarios de ChromeDriver
- **Fácil integración**: Compatible con Claude Desktop y otros clientes MCP

## 📋 Prerrequisitos

- **.NET 9.0** o superior
- **Google Chrome** instalado
- **Claude Desktop** (para usar como cliente MCP)

## 🛠️ Instalación

### 1. Navegar al directorio del proyecto
```bash
cd MCPSeleniumServer\SeleniumMcpServer
```

### 2. Restaurar dependencias
```bash
dotnet restore
```

### 3. Compilar el proyecto
```bash
dotnet build
```

### 4. Publicar el ejecutable
```bash
dotnet publish -c Release -o publish
```

## ⚙️ Configuración en Claude Desktop

### 1. Localizar el archivo de configuración
Edita el archivo de configuración de Claude Desktop:

**Windows:**
```
C:\Users\<tu-usuario>\AppData\Roaming\Claude\claude_desktop_config.json
```

**macOS:**
```
~/Library/Application Support/Claude/claude_desktop_config.json
```

**Linux:**
```
~/.config/Claude/claude_desktop_config.json
```

### 2. Configuraciones recomendadas

#### Opción A: Usar el ejecutable publicado (Recomendado)
```json
{
  "mcpServers": {
    "selenium": {
      "command": "/ruta/completa/al/proyecto/SeleniumMcpServer/publish/SeleniumMcpServer.exe"
    }
  }
}
```

#### Opción B: Usar dotnet run (Para desarrollo)
```json
{
  "mcpServers": {
    "selenium": {
      "command": "dotnet",
      "args": ["run"],
      "cwd": "/ruta/completa/al/proyecto/SeleniumMcpServer"
    }
  }
}
```

#### Opción C: Usando variables de entorno (Más flexible)
```json
{
  "mcpServers": {
    "selenium": {
      "command": "dotnet",
      "args": ["run", "--project", "%SELENIUM_PROJECT_PATH%"]
    }
  }
}
```

### 3. Configuración automática multiplataforma

El proyecto ahora detecta automáticamente la ubicación de Chrome en:
- ✅ **Windows**: Program Files, Program Files (x86), AppData Local
- ✅ **Registro de Windows**: Para instalaciones personalizadas  
- ✅ **Variables de entorno**: CHROME_BIN si está definida

### 4. Verificar la configuración

Después de editar el archivo de configuración:
1. Guarda el archivo
2. Reinicia Claude Desktop completamente
3. Verifica que no hay errores en los logs de Claude Desktop

## 🎯 Uso

Una vez configurado, puedes usar estos comandos en Claude Desktop:

### Iniciar navegador
```
Abre una ventana de Chrome
```

### Navegar a una página web
```
Navega a https://www.google.com
```

### Cerrar el navegador
```
Cierra la sesión del navegador
```

## 🔧 Herramientas disponibles

| Herramienta | Descripción | Parámetros |
|-------------|-------------|------------|
| `start_browser` | Inicia una nueva sesión de navegador | `browser` (chrome/firefox), `headless` (true/false) |
| `navigate` | Navega a una URL específica | `url` (string requerido) |
| `close_session` | Cierra la sesión actual del navegador | Ninguno |

## 📁 Estructura del proyecto

```
MCPSeleniumServer/
├── SeleniumMcpServer/
│   ├── Program.cs              # Punto de entrada y configuración MCP
│   ├── SeleniumService.cs      # Servicio principal para gestión de navegadores
│   ├── SeleniumTools.cs        # Herramientas MCP expuestas
│   ├── SeleniumMcpServer.csproj # Configuración del proyecto
│   └── publish/                # Ejecutables publicados
└── README.md
```

## 🔍 Arquitectura

### SeleniumService
- **Gestión de sesiones**: Mantiene múltiples sesiones de navegador concurrentes
- **Configuración de Chrome**: Optimizada para visibilidad y estabilidad
- **Limpieza automática**: Cierra navegadores al terminar la aplicación

### SeleniumTools
- **Interfaz MCP**: Expone las funcionalidades como herramientas MCP
- **Logging integrado**: Registra todas las operaciones para debugging
- **Manejo de errores**: Captura y reporta errores de manera clara

### Program.cs
- **Hosting**: Configura el servidor MCP con ASP.NET Core
- **Inyección de dependencias**: Registra servicios necesarios
- **Transporte stdio**: Comunicación con clientes MCP

## 🐛 Troubleshooting

### Error: "Browser path is not a valid file"
**Soluciones:**
1. **Verificar instalación de Chrome**: Asegúrate de que Google Chrome esté instalado
2. **Definir variable de entorno**: Crea una variable `CHROME_BIN` con la ruta exacta:
   ```bash
   # Windows (PowerShell)
   $env:CHROME_BIN = "C:\Program Files\Google\Chrome\Application\chrome.exe"
   
   # Windows (Command Prompt)
   set CHROME_BIN=C:\Program Files\Google\Chrome\Application\chrome.exe
   ```
3. **Verificar ubicaciones comunes**: El programa busca Chrome en:
   - `C:\Program Files\Google\Chrome\Application\chrome.exe`
   - `C:\Program Files (x86)\Google\Chrome\Application\chrome.exe`
   - `%LOCALAPPDATA%\Google\Chrome\Application\chrome.exe`

### Error: "No active browser session"
- Ejecuta `start_browser` antes de usar otras herramientas
- Verifica que no haya errores en los logs de Claude Desktop

### El ejecutable no se puede publicar
- Cierra Claude Desktop completamente antes de republicar
- Verifica que no haya procesos `SeleniumMcpServer.exe` ejecutándose

### Configuración multiplataforma
**Para usar en diferentes computadoras:**

#### Opción 1: Script de configuración automática (RECOMENDADO)
```bash
# Ejecutar desde la carpeta del proyecto
setup_portable.bat
```

#### Opción 2: Configuración manual portable
1. **Usar `dotnet run`** en lugar del ejecutable:
   ```json
   {
     "mcpServers": {
       "selenium": {
         "command": "dotnet",
         "args": ["run", "--project", "SeleniumMcpServer"],
         "cwd": "RUTA_COMPLETA_AL_PROYECTO"
       }
     }
   }
   ```

2. **Usar variables de entorno** para máxima portabilidad:
   ```json
   {
     "mcpServers": {
       "selenium": {
         "command": "dotnet",
         "args": ["run", "--project", "SeleniumMcpServer"],
         "cwd": "%MCP_SELENIUM_PATH%"
       }
     }
   }
   ```

3. **Definir la variable** en cada equipo:
   ```powershell
   # Windows PowerShell
   [Environment]::SetEnvironmentVariable("MCP_SELENIUM_PATH", "C:\MisProyectos\MCPSeleniumServer", "User")
   
   # O temporalmente
   $env:MCP_SELENIUM_PATH = "C:\MisProyectos\MCPSeleniumServer"
   ```

#### Opción 3: Para distribución
1. **Copiar** toda la carpeta del proyecto al nuevo equipo
2. **Ejecutar** el script `setup_portable.bat`
3. **Reiniciar** Claude Desktop

#### Ventajas de cada configuración:
- **Ejecutable publicado**: Más rápido, pero requiere rutas absolutas
- **dotnet run**: Más flexible, funciona en cualquier equipo con .NET
- **Variables de entorno**: Máxima portabilidad, fácil de cambiar

## 📦 Archivos de configuración incluidos

- `claude_desktop_config_examples.json`: Ejemplos de configuración para diferentes escenarios
- `setup_portable.bat`: Script de configuración automática
- `README.md`: Esta documentación completa
3. **Usa la opción `dotnet run`** para máxima portabilidad

### Configuración portable recomendada
```json
{
  "mcpServers": {
    "selenium": {
      "command": "dotnet",
      "args": ["run"],
      "cwd": "./SeleniumMcpServer"
    }
  }
}
```

## 🤝 Contribuciones

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## 📜 Licencia

Este proyecto está bajo la Licencia MIT. Ver el archivo `LICENSE` para más detalles.

## 🔗 Enlaces útiles

- [Model Context Protocol Documentation](https://modelcontextprotocol.io/)
- [Selenium WebDriver Documentation](https://selenium-python.readthedocs.io/)
- [Claude Desktop](https://claude.ai/download)

## 📝 Changelog

### v1.0.0
- ✅ Implementación inicial del servidor MCP
- ✅ Soporte para Chrome y Firefox
- ✅ Herramientas básicas: start_browser, navigate, close_session
- ✅ Configuración optimizada para ventana visible
- ✅ Supresión de logs innecesarios