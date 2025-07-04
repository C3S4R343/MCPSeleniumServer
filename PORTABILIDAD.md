# 🚀 Guía de Portabilidad MCP Selenium Server

## 📋 Resumen
Esta guía te ayuda a usar el servidor MCP Selenium en diferentes computadoras sin tener que modificar código ni configuraciones manualmente.

## 🎯 Configuración Actual (Ya aplicada)
Tu archivo `claude_desktop_config.json` ahora está configurado para usar `dotnet run`, que es la opción más portable:

```json
{
  "mcpServers": {
    "selenium": {
      "command": "dotnet",
      "args": ["run", "--project", "SeleniumMcpServer"],
      "cwd": "C:\\Users\\cesar\\OneDrive\\Documentos\\ProyectosApex\\MCPSeleniumServer"
    }
  }
}
```

## 🔄 Para usar en otro equipo

### Método 1: Automático (RECOMENDADO)
1. **Copia** toda la carpeta `MCPSeleniumServer` al nuevo equipo
2. **Abre** PowerShell como administrador en la carpeta del proyecto
3. **Ejecuta**: `.\setup_portable.bat`
4. **Reinicia** Claude Desktop

### Método 2: Manual
1. **Copia** la carpeta del proyecto al nuevo equipo
2. **Instala** .NET si no está instalado: https://dotnet.microsoft.com/download
3. **Edita** el archivo `claude_desktop_config.json` en el nuevo equipo:
   - Ubicación: `%APPDATA%\Claude\claude_desktop_config.json`
   - Cambia solo la ruta en `"cwd"` por la nueva ubicación

### Método 3: Variables de Entorno (MÁXIMA PORTABILIDAD)
1. **Define** la variable `MCP_SELENIUM_PATH` en cada equipo:
   ```powershell
   # PowerShell (permanente)
   [Environment]::SetEnvironmentVariable("MCP_SELENIUM_PATH", "C:\TuRuta\MCPSeleniumServer", "User")
   
   # PowerShell (temporal)
   $env:MCP_SELENIUM_PATH = "C:\TuRuta\MCPSeleniumServer"
   ```

2. **Usa** esta configuración en `claude_desktop_config.json`:
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

## 📝 Verificación

Para verificar que todo funciona correctamente:

1. **Abre** PowerShell en la carpeta del proyecto
2. **Ejecuta**: `dotnet run --project SeleniumMcpServer`
3. **Debería** mostrar: `MCP Server is running on stdio`
4. **Presiona** Ctrl+C para detener

## 🔧 Requisitos por equipo

- **Windows 10/11**
- **.NET 9.0** o superior
- **Google Chrome** (se detecta automáticamente)
- **Claude Desktop** instalado

## 🚨 Problemas comunes

### "dotnet no se reconoce como comando"
**Solución**: Instala .NET SDK desde https://dotnet.microsoft.com/download

### "Chrome no encontrado"
**Solución**: 
1. Instala Google Chrome
2. O define la variable `CHROME_BIN` con la ruta correcta:
   ```powershell
   $env:CHROME_BIN = "C:\Program Files\Google\Chrome\Application\chrome.exe"
   ```

### "No se puede acceder al archivo"
**Solución**: Cierra Claude Desktop completamente antes de copiar archivos

## 📊 Comparación de métodos

| Método | Velocidad | Portabilidad | Facilidad |
|--------|-----------|--------------|-----------|
| Automático | ⭐⭐⭐ | ⭐⭐⭐⭐⭐ | ⭐⭐⭐⭐⭐ |
| Manual | ⭐⭐⭐ | ⭐⭐⭐⭐ | ⭐⭐⭐ |
| Variables | ⭐⭐⭐ | ⭐⭐⭐⭐⭐ | ⭐⭐ |
| Ejecutable | ⭐⭐⭐⭐⭐ | ⭐⭐ | ⭐⭐ |

## 📞 Soporte

Si tienes problemas:
1. Revisa los logs de Claude Desktop
2. Verifica que .NET esté instalado: `dotnet --version`
3. Comprueba que Chrome esté disponible
4. Asegúrate de que la ruta del proyecto sea correcta

## 🎉 ¡Listo!

Con estas configuraciones, puedes usar tu servidor MCP Selenium en cualquier equipo Windows sin modificar código. Solo necesitas copiar la carpeta y ejecutar el script de configuración.
