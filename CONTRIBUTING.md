# Contribuye al MCP Selenium Server

¡Gracias por tu interés en contribuir! Este proyecto es open source y todas las contribuciones son bienvenidas.

## 🚀 Comenzando

1. **Fork** el repositorio
2. **Clona** tu fork localmente
3. **Crea** una rama para tu contribución
4. **Haz** tus cambios
5. **Envía** un pull request

## 🛠️ Configuración de Desarrollo

```bash
# Clonar tu fork
git clone https://github.com/TU_USUARIO/mcp-selenium-server.git
cd mcp-selenium-server

# Agregar upstream
git remote add upstream https://github.com/USUARIO_ORIGINAL/mcp-selenium-server.git

# Instalar dependencias
dotnet restore
```

## 📋 Tipos de Contribuciones

### 🐛 Reportar Bugs
- Usa el template de bug report
- Incluye pasos para reproducir
- Incluye información del sistema

### 💡 Proponer Funcionalidades
- Usa el template de feature request
- Describe el caso de uso
- Propón la implementación

### 🔧 Código
- Arreglar bugs
- Agregar nuevas herramientas MCP
- Mejorar documentación
- Optimizar rendimiento

## 🎯 Áreas de Contribución

### Nuevas Herramientas Selenium
- `click_element` - Hacer clic en elementos
- `send_keys` - Enviar texto a campos
- `take_screenshot` - Capturar pantalla
- `find_element` - Encontrar elementos
- `wait_for_element` - Esperar elementos

### Mejoras de Configuración
- Soporte para más navegadores
- Configuración de proxy
- Manejo de certificados
- Opciones de performance

### Documentación
- Traducir documentación
- Agregar ejemplos
- Mejorar guías de instalación

## 🧪 Pruebas

```bash
# Ejecutar pruebas
dotnet test

# Verificar funcionamiento
dotnet run --project SeleniumMcpServer
```

## 📝 Estándares de Código

### Convenciones C#
- Usar PascalCase para métodos públicos
- Usar camelCase para variables privadas
- Documentar métodos públicos con XML comments
- Seguir principios SOLID

### Ejemplo de Método
```csharp
/// <summary>
/// Hace clic en un elemento web específico.
/// </summary>
/// <param name="selector">Selector CSS del elemento</param>
/// <param name="sessionId">ID de sesión opcional</param>
/// <returns>Resultado de la operación</returns>
public async Task<ClickResult> ClickElementAsync(string selector, string? sessionId = null)
{
    // Implementación
}
```

## 🔄 Proceso de Pull Request

1. **Actualiza** tu fork con upstream
2. **Crea** una rama descriptiva
3. **Implementa** tu cambio
4. **Agrega** pruebas si es necesario
5. **Actualiza** documentación
6. **Envía** pull request

### Ejemplo de Comandos
```bash
# Actualizar fork
git checkout main
git pull upstream main

# Crear rama
git checkout -b feature/nueva-herramienta

# Hacer cambios y commit
git add .
git commit -m "feat: agregar herramienta click_element"

# Push y crear PR
git push origin feature/nueva-herramienta
```

## 📋 Checklist de PR

- [ ] Código sigue las convenciones
- [ ] Pruebas pasan
- [ ] Documentación actualizada
- [ ] Commit messages descriptivos
- [ ] No hay conflictos con main

## 🏷️ Convenciones de Commits

Usa [Conventional Commits](https://www.conventionalcommits.org/):

```
feat: agregar nueva herramienta MCP
fix: corregir detección de Chrome
docs: actualizar guía de instalación
style: formatear código
refactor: mejorar estructura de clases
test: agregar pruebas unitarias
chore: actualizar dependencias
```

## 🤝 Código de Conducta

- Sé respetuoso con otros contribuyentes
- Mantén discusiones constructivas
- Ayuda a otros con sus preguntas
- Reporta comportamientos inapropiados

## 🛡️ Seguridad

Si encuentras vulnerabilidades de seguridad:
1. **NO** las reportes públicamente
2. Envía un email a [seguridad@ejemplo.com]
3. Describe el problema en detalle
4. Espera respuesta antes de divulgar

## 📞 Obtener Ayuda

- **Discussions**: Para preguntas generales
- **Issues**: Para bugs y feature requests
- **Discord**: [Link al servidor] (si existe)
- **Email**: [email@ejemplo.com]

## 🎉 Reconocimientos

Todos los contribuyentes serán reconocidos en:
- README.md
- CONTRIBUTORS.md
- Release notes

¡Gracias por hacer que MCP Selenium Server sea mejor! 🚀
