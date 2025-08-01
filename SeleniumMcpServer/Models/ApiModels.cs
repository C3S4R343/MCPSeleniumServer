using System.ComponentModel.DataAnnotations;

namespace SeleniumMcpServer.Models;

/// <summary>
/// Modelo para solicitar el inicio de un navegador
/// </summary>
public class StartBrowserRequest
{
    /// <summary>
    /// Tipo de navegador (chrome o firefox)
    /// </summary>
    public string Browser { get; set; } = "chrome";
    
    /// <summary>
    /// Si debe ejecutarse en modo headless
    /// </summary>
    public bool Headless { get; set; } = false;
}

/// <summary>
/// Modelo para solicitar navegación a una URL
/// </summary>
public class NavigateRequest
{
    /// <summary>
    /// URL a la que navegar
    /// </summary>
    [Required(ErrorMessage = "URL es requerida")]
    [Url(ErrorMessage = "Debe ser una URL válida")]
    public string Url { get; set; } = string.Empty;
}

/// <summary>
/// Modelo para respuestas de la API
/// </summary>
public class ApiResponse
{
    /// <summary>
    /// Indica si la operación fue exitosa
    /// </summary>
    public bool Success { get; set; }
    
    /// <summary>
    /// Mensaje de respuesta
    /// </summary>
    public string Message { get; set; } = string.Empty;
    
    /// <summary>
    /// Datos adicionales (opcional)
    /// </summary>
    public object? Data { get; set; }
}

/// <summary>
/// Modelo para respuesta de inicio de navegador
/// </summary>
public class StartBrowserResponse : ApiResponse
{
    /// <summary>
    /// ID de la sesión del navegador
    /// </summary>
    public string? SessionId { get; set; }
}

/// <summary>
/// Modelo para captura de pantalla
/// </summary>
public class ScreenshotRequest
{
    /// <summary>
    /// Nombre del archivo (opcional)
    /// </summary>
    public string? FileName { get; set; }
    
    /// <summary>
    /// Si debe devolver la imagen como base64
    /// </summary>
    public bool ReturnBase64 { get; set; } = true;
}
