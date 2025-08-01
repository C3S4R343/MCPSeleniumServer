using Microsoft.AspNetCore.Mvc;
using SeleniumMcpServer.Models;
using OpenQA.Selenium;

namespace SeleniumMcpServer.Controllers;

/// <summary>
/// Controlador de API REST para operaciones de Selenium
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class SeleniumController : ControllerBase
{
    private readonly SeleniumService _seleniumService;
    private readonly ILogger<SeleniumController> _logger;

    /// <summary>
    /// Constructor del controlador de Selenium
    /// </summary>
    /// <param name="seleniumService">Servicio de Selenium</param>
    /// <param name="logger">Logger para registrar eventos</param>
    public SeleniumController(SeleniumService seleniumService, ILogger<SeleniumController> logger)
    {
        _seleniumService = seleniumService;
        _logger = logger;
    }

    /// <summary>
    /// Inicia una nueva sesión de navegador
    /// </summary>
    /// <param name="request">Configuración del navegador</param>
    /// <returns>Información de la sesión creada</returns>
    [HttpPost("start-browser")]
    [ProducesResponseType(typeof(StartBrowserResponse), 200)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    public IActionResult StartBrowser([FromBody] StartBrowserRequest request)
    {
        try
        {
            _logger.LogInformation("Starting browser: {Browser}, Headless: {Headless}", 
                request.Browser, request.Headless);
            
            var sessionId = _seleniumService.StartBrowser(request.Browser, request.Headless);
            
            return Ok(new StartBrowserResponse
            {
                Success = true,
                Message = $"Browser session started successfully",
                SessionId = sessionId
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to start browser");
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = $"Failed to start browser: {ex.Message}"
            });
        }
    }

    /// <summary>
    /// Navega a una URL específica
    /// </summary>
    /// <param name="request">URL de destino</param>
    /// <returns>Resultado de la navegación</returns>
    [HttpPost("navigate")]
    [ProducesResponseType(typeof(ApiResponse), 200)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    public IActionResult Navigate([FromBody] NavigateRequest request)
    {
        try
        {
            var driver = _seleniumService.GetCurrentDriver();
            driver.Navigate().GoToUrl(request.Url);
            
            _logger.LogInformation("Navigated to: {Url}", request.Url);
            
            return Ok(new ApiResponse
            {
                Success = true,
                Message = $"Successfully navigated to {request.Url}",
                Data = new { url = request.Url, title = driver.Title }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to navigate to {Url}", request.Url);
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = $"Failed to navigate: {ex.Message}"
            });
        }
    }

    /// <summary>
    /// Obtiene el título de la página actual
    /// </summary>
    /// <returns>Título de la página</returns>
    [HttpGet("page-title")]
    [ProducesResponseType(typeof(ApiResponse), 200)]
    public IActionResult GetPageTitle()
    {
        try
        {
            var driver = _seleniumService.GetCurrentDriver();
            var title = driver.Title;
            
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Page title retrieved successfully",
                Data = new { title = title }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get page title");
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = $"Failed to get page title: {ex.Message}"
            });
        }
    }

    /// <summary>
    /// Toma una captura de pantalla
    /// </summary>
    /// <param name="request">Configuración de la captura</param>
    /// <returns>Captura de pantalla en base64 o información del archivo</returns>
    [HttpPost("screenshot")]
    [ProducesResponseType(typeof(ApiResponse), 200)]
    public IActionResult TakeScreenshot([FromBody] ScreenshotRequest request)
    {
        try
        {
            var driver = _seleniumService.GetCurrentDriver();
            var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            
            if (request.ReturnBase64)
            {
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Screenshot taken successfully",
                    Data = new { base64Image = screenshot.AsBase64EncodedString }
                });
            }
            else
            {
                var fileName = request.FileName ?? $"screenshot_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
                screenshot.SaveAsFile(filePath);
                
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Screenshot saved successfully",
                    Data = new { filePath = filePath, fileName = fileName }
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to take screenshot");
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = $"Failed to take screenshot: {ex.Message}"
            });
        }
    }

    /// <summary>
    /// Cierra la sesión actual del navegador
    /// </summary>
    /// <returns>Resultado del cierre</returns>
    [HttpPost("close-session")]
    [ProducesResponseType(typeof(ApiResponse), 200)]
    public IActionResult CloseSession()
    {
        try
        {
            _seleniumService.CloseSession();
            
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Browser session closed successfully"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to close session");
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = $"Failed to close session: {ex.Message}"
            });
        }
    }

    /// <summary>
    /// Obtiene el estado de la sesión actual
    /// </summary>
    /// <returns>Información del estado</returns>
    [HttpGet("status")]
    [ProducesResponseType(typeof(ApiResponse), 200)]
    public IActionResult GetStatus()
    {
        try
        {
            var hasSession = _seleniumService.HasActiveSession();
            string currentUrl = "";
            string title = "";
            
            if (hasSession)
            {
                var driver = _seleniumService.GetCurrentDriver();
                currentUrl = driver.Url;
                title = driver.Title;
            }
            
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Status retrieved successfully",
                Data = new 
                { 
                    hasActiveSession = hasSession,
                    currentUrl = currentUrl,
                    title = title,
                    timestamp = DateTime.Now
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get status");
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = $"Failed to get status: {ex.Message}"
            });
        }
    }
}