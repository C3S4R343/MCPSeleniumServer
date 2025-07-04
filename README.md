# MCP Selenium Server

Un servidor MCP (Model Context Protocol) en C# que expone herramientas de automatización web usando Selenium WebDriver. Permite a Claude Desktop y otros clientes MCP controlar navegadores web de forma programática.

## 🚀 Características

- **Gestión de sesiones**: Múltiples sesiones de navegador concurrentes
- **Soporte multi-navegador**: Chrome y Firefox
- **Configuración optimizada**: Detección automática de Chrome
- **Comunicación MCP limpia**: Sin interferencias de MSBuild
- **Multiplataforma**: Windows, Linux, macOS

## 📋 Requisitos

- **.NET 9.0 SDK** o superior
- **Google Chrome** (se detecta automáticamente)
- **Claude Desktop** u otro cliente MCP

## 🛠️ Instalación Rápida

### 1. Clonar el repositorio
```bash
git clone https://github.com/TU_USUARIO/mcp-selenium-server.git
cd mcp-selenium-server
```

### 2. Configuración automática (Windows)
```bash
# Ejecutar script de configuración
.\install_new_computer.bat
```

### 3. Reiniciar Claude Desktop
- Cierra Claude Desktop completamente
- Vuelve a abrirlo
- Prueba comandos como: "start selenium browser"

#### Compilar el proyecto
```bash
dotnet build
```

#### Publicar ejecutable optimizado
```bash
dotnet publish -c Release -o publish --self-contained false
```

#### Configurar Claude Desktop
Edita `%APPDATA%\Claude\claude_desktop_config.json`:

```json
{
  "mcpServers": {
    "selenium": {
      "command": "RUTA_COMPLETA\\publish\\SeleniumMcpServer.exe"
    }
  }
}
```

## 🔧 Herramientas MCP Disponibles

### `start_browser`
Inicia una nueva sesión de navegador
```json
{
  "browserType": "chrome",
  "headless": false
}
```

### `navigate`
Navega a una URL específica
```json
{
  "url": "https://example.com"
}
```

### `close_session`
Cierra la sesión de navegador actual
```json
{
  "sessionId": "opcional"
}
```

## 📁 Estructura del Proyecto

```
mcp-selenium-server/
├── SeleniumMcpServer/
│   ├── Program.cs              # Punto de entrada
│   ├── SeleniumService.cs      # Servicio de gestión de navegadores
│   ├── SeleniumTools.cs        # Herramientas MCP
│   └── SeleniumMcpServer.csproj
├── publish/                    # Ejecutables publicados
├── install_new_computer.bat    # Script de instalación automática
├── setup_portable.bat          # Script de configuración portable
├── fix_and_build.bat          # Script de reparación
├── SOLUCION_PROBLEMAS.md      # Guía de solución de problemas
└── README.md
```

## � Instalación en Nueva Computadora

### ⚡ Método Rápido
1. **Clona** el repositorio
2. **Ejecuta** `install_new_computer.bat` (Windows)
3. **Reinicia** Claude Desktop

### 🛠️ Método Manual
Ver [INSTALACION_NUEVA_COMPUTADORA.md](INSTALACION_NUEVA_COMPUTADORA.md) para instrucciones detalladas.

## 🐛 Solución de Problemas

### Error: "Unexpected token 'M'"
```bash
# Ejecutar script de reparación
.\fix_and_build.bat
```

### Chrome no encontrado
```bash
# Definir variable de entorno
set CHROME_BIN=C:\Program Files\Google\Chrome\Application\chrome.exe
```

### Problemas de compilación
```bash
# Limpiar y recompilar
dotnet clean
dotnet build
```

## 📝 Configuraciones de Ejemplo

Ver `claude_desktop_config_examples.json` para diferentes configuraciones.

## 🤝 Contribuciones

Las contribuciones son bienvenidas. Por favor:

1. Fork el repositorio
2. Crea una rama para tu feature (`git checkout -b feature/nueva-funcionalidad`)
3. Commit tus cambios (`git commit -am 'Agregar nueva funcionalidad'`)
4. Push a la rama (`git push origin feature/nueva-funcionalidad`)
5. Abre un Pull Request

## 📄 Licencia

Este proyecto está licenciado bajo la Licencia MIT - ver el archivo [LICENSE](LICENSE) para más detalles.

## 🔗 Enlaces Útiles

- [Model Context Protocol](https://modelcontextprotocol.io/)
- [Selenium WebDriver](https://selenium.dev/)
- [Claude Desktop](https://claude.ai/download)

## 👥 Créditos

Desarrollado con ❤️ para la comunidad MCP.

---

**¿Necesitas ayuda?** Consulta [SOLUCION_PROBLEMAS.md](SOLUCION_PROBLEMAS.md) para guías detalladas.
