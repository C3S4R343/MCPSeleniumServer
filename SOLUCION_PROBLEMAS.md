# 🔧 Solución Rápida de Problemas - MCP Selenium Server

## ⚠️ Problema Común: "Unexpected token 'M'"

Si ves este error en los logs de Claude Desktop:
```
Unexpected token 'M', "MSBUILD :"... is not valid JSON
```

### 🚨 Causa
El problema ocurre cuando MSBuild envía mensajes en español que no son JSON válido, interfiriendo con la comunicación MCP.

### ✅ Solución DEFINITIVA

**¡NUEVA SOLUCIÓN OPTIMIZADA!** - Usa ejecutable directo (sin MSBuild)

1. **Cierra Claude Desktop completamente**
2. **Ejecuta el script de reparación optimizada**:
   ```bash
   # Desde la carpeta del proyecto
   .\fix_and_build.bat
   ```
3. **Reinicia Claude Desktop**

**Esta solución:**
- ✅ Elimina completamente los mensajes de MSBuild
- ✅ Inicio más rápido del servidor
- ✅ Comunicación MCP limpia
- ✅ Sin errores de JSON

### 🔧 Solución Manual (si es necesario)

Si necesitas republicar manualmente:

```powershell
# 1. Terminar procesos existentes
taskkill /F /IM "SeleniumMcpServer.exe" /T

# 2. Ir a la carpeta del proyecto
cd "c:\Users\cesar\OneDrive\Documentos\ProyectosApex\MCPSeleniumServer"

# 3. Limpiar y publicar
dotnet clean
dotnet publish -c Release -o publish --self-contained false
```

### 🔄 Para otros equipos (Portabilidad)

**Opción 1: Usar ejecutable (recomendado)**
```json
{
  "mcpServers": {
    "selenium": {
      "command": "RUTA_COMPLETA\\publish\\SeleniumMcpServer.exe"
    }
  }
}
```

**Opción 2: Usar dotnet run (portable pero puede tener problemas JSON)**
```json
{
  "mcpServers": {
    "selenium": {
      "command": "dotnet",
      "args": ["run", "--project", "SeleniumMcpServer", "--verbosity", "quiet"],
      "cwd": "RUTA_COMPLETA",
      "env": {
        "DOTNET_CLI_UI_LANGUAGE": "en"
      }
    }
  }
}
```

### 📝 Configuración OPTIMIZADA (Aplicada)

Tu archivo `claude_desktop_config.json` ahora usa el ejecutable optimizado:

```json
{
  "mcpServers": {
    "selenium": {
      "command": "C:\\Users\\cesar\\OneDrive\\Documentos\\ProyectosApex\\MCPSeleniumServer\\publish\\SeleniumMcpServer.exe"
    }
  }
}
```

**Ventajas:**
- 🚀 Inicio instantáneo
- 🚫 Sin mensajes de MSBuild
- ✅ Comunicación MCP pura
- 🔧 Más estable

## 🔍 Verificación

Para verificar que todo funciona:

1. **Compilación exitosa**: No debería haber errores al compilar
2. **Servidor MCP**: Debe mostrar "MCP Server is running on stdio"
3. **Claude Desktop**: Debe mostrar "Server started and connected successfully"

## 🛠️ Archivos de Ayuda

- `fix_and_build.bat` - Script de reparación automática
- `setup_portable.bat` - Configuración para nuevos equipos
- `claude_desktop_config_examples.json` - Ejemplos de configuración
- `PORTABILIDAD.md` - Guía de uso en diferentes equipos

## 💡 Consejos

- **Siempre** cierra Claude Desktop antes de recompilar
- **Usa** el script `fix_and_build.bat` cuando haya problemas
- **Reinicia** Claude Desktop después de cambios
- **Verifica** que .NET esté instalado: `dotnet --version`

## 📞 Si Sigue Fallando

1. Verifica que Chrome esté instalado
2. Comprueba que .NET 9.0 esté instalado
3. Ejecuta como administrador si es necesario
4. Revisa los logs completos de Claude Desktop

---

**¡El servidor MCP Selenium ahora está optimizado y es más estable!** 🎉
