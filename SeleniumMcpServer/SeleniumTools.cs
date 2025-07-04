using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace SeleniumMcpServer;

/// <summary>
/// Clase que expone las herramientas de Selenium como funciones del Model Context Protocol (MCP).
/// Esta clase actúa como la interfaz entre el protocolo MCP y el servicio de Selenium,
/// permitiendo que clientes MCP (como Claude Desktop) controlen navegadores web.
/// </summary>
[McpServerToolType] // Atributo que indica que esta clase contiene herramientas MCP
public class SeleniumTools
{
    /// <summary>
    /// Instancia del servicio de Selenium para gestionar navegadores
    /// </summary>
    private readonly SeleniumService _seleniumService;
    
    /// <summary>
    /// Logger para registrar operaciones y errores
    /// </summary>
    private readonly ILogger<SeleniumTools> _logger;

    /// <summary>
    /// Constructor que recibe las dependencias necesarias a través de inyección de dependencias
    /// </summary>
    /// <param name="seleniumService">Servicio para gestionar sesiones de navegador</param>
    /// <param name="logger">Logger para registrar eventos</param>
    public SeleniumTools(SeleniumService seleniumService, ILogger<SeleniumTools> logger)
    {
        _seleniumService = seleniumService;
        _logger = logger;
    }

    /// <summary>
    /// Herramienta MCP para iniciar una nueva sesión de navegador.
    /// Esta función es expuesta al cliente MCP y puede ser invocada desde Claude Desktop.
    /// </summary>
    /// <param name="browser">Tipo de navegador a iniciar (chrome o firefox)</param>
    /// <param name="headless">Si true, ejecuta el navegador sin interfaz gráfica</param>
    /// <returns>Mensaje de confirmación con el ID de sesión</returns>
    [McpServerTool] // Atributo que marca este método como una herramienta MCP
    [Description("Starts a new browser session")] // Descripción que ve el cliente MCP
    public string StartBrowser(
        [Description("Browser type to start (chrome or firefox)")] string browser = "chrome",
        [Description("Whether to run in headless mode")] bool headless = false)
    {
        try
        {
            // Delegar la creación del navegador al servicio de Selenium
            var sessionId = _seleniumService.StartBrowser(browser, headless);
            
            // Registrar la operación exitosa en los logs
            _logger.LogInformation("Started {Browser} browser session: {SessionId}", browser, sessionId);
            
            // Retornar mensaje de confirmación al cliente MCP
            return $"Browser session started: {sessionId}";
        }
        catch (Exception ex)
        {
            // Registrar el error y relanzar la excepción para que el cliente MCP la reciba
            _logger.LogError(ex, "Failed to start browser");
            throw;
        }
    }

    /// <summary>
    /// Herramienta MCP para navegar a una URL específica.
    /// Utiliza la sesión de navegador actualmente activa.
    /// </summary>
    /// <param name="url">URL a la que navegar</param>
    /// <returns>Mensaje de confirmación</returns>
    [McpServerTool]
    [Description("Navigates to a URL")]
    public string Navigate([Description("URL to navigate to")] string url)
    {
        try
        {
            // Obtener el driver de la sesión activa
            var driver = _seleniumService.GetCurrentDriver();
            
            // Navegar a la URL especificada
            driver.Navigate().GoToUrl(url);
            
            // Registrar la navegación exitosa
            _logger.LogInformation("Navigated to: {Url}", url);
            
            return $"Navigated to: {url}";
        }
        catch (Exception ex)
        {
            // Registrar y relanzar errores (como no tener sesión activa)
            _logger.LogError(ex, "Failed to navigate to {Url}", url);
            throw;
        }
    }

    /// <summary>
    /// Herramienta MCP para cerrar la sesión actual del navegador.
    /// Cierra la ventana del navegador y limpia la sesión.
    /// </summary>
    /// <returns>Mensaje de confirmación</returns>
    [McpServerTool]
    [Description("Closes the current browser session")]
    public string CloseSession()
    {
        try
        {
            // Cerrar la sesión actual del navegador
            _seleniumService.CloseSession();
            
            // Registrar el cierre exitoso
            _logger.LogInformation("Browser session closed");
            
            return "Browser session closed";
        }
        catch (Exception ex)
        {
            // Registrar y relanzar errores
            _logger.LogError(ex, "Failed to close browser session");
            throw;
        }
    }
}