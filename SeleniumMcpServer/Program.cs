// Importaciones necesarias para el servidor MCP y hosting
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using SeleniumMcpServer;
using System.Globalization;

// === CONFIGURACIÓN DEL SERVIDOR MCP SELENIUM ===
// Este es el punto de entrada principal del servidor MCP Selenium
// que permite la integración con Claude Desktop y otros clientes MCP

// Configurar la cultura para asegurar formato inglés en todos los mensajes
CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

// Redirigir Console.Out a stderr para evitar interferencia con MCP
var stderr = Console.Error;
Console.SetOut(stderr);

// Crear el builder de la aplicación host
var builder = Host.CreateApplicationBuilder(args);

// === CONFIGURACIÓN DE LOGGING ===
// Configurar el sistema de logging para dirigir todos los logs a stderr
// Esto es importante porque el protocolo MCP usa stdout para comunicación
builder.Logging.AddConsole(consoleLogOptions =>
{
    // Enviar todos los logs (desde Trace en adelante) a stderr
    // para no interferir con la comunicación MCP en stdout
    consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
});

// === REGISTRO DE SERVICIOS ===
// Registrar el SeleniumService como singleton para mantener el estado
// de las sesiones de navegador durante toda la vida de la aplicación
builder.Services.AddSingleton<SeleniumService>();

// === CONFIGURACIÓN DEL SERVIDOR MCP ===
// Configurar el servidor MCP con las opciones necesarias
builder.Services
    .AddMcpServer()                    // Agregar servicios base de MCP
    .WithStdioServerTransport()        // Usar transporte stdio (stdin/stdout) para comunicación
    .WithToolsFromAssembly();          // Escanear automáticamente el ensamblado en busca de herramientas MCP

// Construir la aplicación host con toda la configuración
var host = builder.Build();

// === MANEJO DEL CIERRE LIMPIO ===
// Configurar un manejador para cerrar navegadores cuando se cancele la aplicación (Ctrl+C)
Console.CancelKeyPress += (sender, e) =>
{
    e.Cancel = true; // Cancelar el cierre inmediato del proceso
    
    // Obtener el servicio de Selenium y cerrar todas las sesiones de navegador
    host.Services.GetService<SeleniumService>()?.Dispose();
    
    // Terminar la aplicación de forma limpia
    Environment.Exit(0);
};

// === SUPRESIÓN DE STDOUT ===
// Redirigir stdout a null para evitar que mensajes no-JSON interfieran con MCP
// El protocolo MCP requiere que solo se envíen mensajes JSON válidos por stdout
Console.SetOut(TextWriter.Null);

// === INICIO DEL SERVIDOR ===
// Iniciar el servidor MCP y mantenerlo ejecutándose
// Este método bloquea hasta que la aplicación se termina
await host.RunAsync();
