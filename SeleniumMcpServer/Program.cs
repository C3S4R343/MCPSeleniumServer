// Importaciones necesarias para el servidor MCP y hosting
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ModelContextProtocol.Server;
using SeleniumMcpServer;
using System.Globalization;

// === CONFIGURACIÓN DEL SERVIDOR HÍBRIDO (MCP + API REST) ===
// Este servidor puede funcionar tanto como MCP Server para Claude Desktop
// como API REST para GitHub Copilot y otras herramientas

// Configurar la cultura para asegurar formato inglés en todos los mensajes
CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

// Detectar el modo de ejecución basado en argumentos
var commandArgs = Environment.GetCommandLineArgs();
var isApiMode = commandArgs.Contains("--api") || commandArgs.Contains("-a");
var port = GetPortFromArgs(commandArgs) ?? 5000;

if (isApiMode)
{
    // === MODO API REST ===
    await RunAsApiServer(port);
}
else
{
    // === MODO MCP (Default) ===
    await RunAsMcpServer();
}

// === FUNCIONES AUXILIARES ===

static async Task RunAsApiServer(int port)
{
    var builder = WebApplication.CreateBuilder();
    
    // Configurar servicios para API REST
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        { 
            Title = "Selenium MCP Server API", 
            Version = "v1",
            Description = "API REST para automatización web con Selenium"
        });
        
        // Incluir comentarios XML para documentación
        var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath))
        {
            c.IncludeXmlComments(xmlPath);
        }
    });
    
    // Configurar CORS para permitir acceso desde diferentes orígenes
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });
    
    // Registrar el SeleniumService como singleton
    builder.Services.AddSingleton<SeleniumService>();
    
    // Configurar logging
    builder.Services.AddLogging(logging =>
    {
        logging.AddConsole();
        logging.SetMinimumLevel(LogLevel.Information);
    });
    
    var app = builder.Build();
    
    // Configurar middleware
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Selenium MCP Server API v1");
            c.RoutePrefix = string.Empty; // Swagger en la raíz
        });
    }
    
    app.UseCors("AllowAll");
    app.UseRouting();
    app.MapControllers();
    
    // Configurar el puerto
    app.Urls.Add($"http://localhost:{port}");
    
    Console.WriteLine($"🚀 Selenium API Server iniciado en http://localhost:{port}");
    Console.WriteLine($"📖 Documentación Swagger disponible en http://localhost:{port}");
    Console.WriteLine("Presiona Ctrl+C para salir...");
    
    // Manejo limpio del cierre
    var cancellationTokenSource = new CancellationTokenSource();
    Console.CancelKeyPress += (sender, e) =>
    {
        e.Cancel = true;
        app.Services.GetService<SeleniumService>()?.Dispose();
        cancellationTokenSource.Cancel();
    };
    
    await app.RunAsync(cancellationTokenSource.Token);
}

static async Task RunAsMcpServer()
{
    // Redirigir Console.Out a stderr para evitar interferencia con MCP
    var stderr = Console.Error;
    Console.SetOut(stderr);
    
    // Crear el builder de la aplicación host
    var builder = Host.CreateApplicationBuilder();
    
    // === CONFIGURACIÓN DE LOGGING ===
    builder.Logging.AddConsole(consoleLogOptions =>
    {
        consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
    });
    
    // === REGISTRO DE SERVICIOS ===
    builder.Services.AddSingleton<SeleniumService>();
    
    // === CONFIGURACIÓN DEL SERVIDOR MCP ===
    builder.Services
        .AddMcpServer()
        .WithStdioServerTransport()
        .WithToolsFromAssembly();
    
    var host = builder.Build();
    
    // === MANEJO DEL CIERRE LIMPIO ===
    Console.CancelKeyPress += (sender, e) =>
    {
        e.Cancel = true;
        host.Services.GetService<SeleniumService>()?.Dispose();
        Environment.Exit(0);
    };
    
    // === SUPRESIÓN DE STDOUT ===
    Console.SetOut(TextWriter.Null);
    
    // === INICIO DEL SERVIDOR ===
    await host.RunAsync();
}

static int? GetPortFromArgs(string[] args)
{
    for (int i = 0; i < args.Length - 1; i++)
    {
        if (args[i] == "--port" || args[i] == "-p")
        {
            if (int.TryParse(args[i + 1], out int port))
            {
                return port;
            }
        }
    }
    return null;
}
