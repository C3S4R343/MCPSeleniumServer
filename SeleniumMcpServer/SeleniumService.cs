using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System.Collections.Concurrent;

namespace SeleniumMcpServer;

/// <summary>
/// Servicio principal para gestión de sesiones de navegadores web usando Selenium.
/// Maneja múltiples sesiones concurrentes y proporciona configuración optimizada
/// para pruebas automatizadas con ventana visible.
/// </summary>
public class SeleniumService : IDisposable
{
    /// <summary>
    /// Diccionario thread-safe que almacena todas las sesiones activas de navegadores.
    /// La clave es el ID de sesión (GUID) y el valor es la instancia de IWebDriver.
    /// </summary>
    private readonly ConcurrentDictionary<string, IWebDriver> _drivers = new();
    
    /// <summary>
    /// ID de la sesión de navegador actualmente activa.
    /// Se usa para operaciones que no especifican una sesión particular.
    /// </summary>
    private string? _currentSessionId;

    /// <summary>
    /// Inicia una nueva sesión de navegador con la configuración especificada.
    /// </summary>
    /// <param name="browserType">Tipo de navegador a iniciar ("chrome" o "firefox")</param>
    /// <param name="headless">Si true, ejecuta el navegador sin interfaz gráfica</param>
    /// <returns>ID único de la sesión creada</returns>
    /// <exception cref="ArgumentException">Se lanza cuando el tipo de navegador no es compatible</exception>
    public string StartBrowser(string browserType = "chrome", bool headless = false)
    {
        // Generar un ID único para esta sesión de navegador
        var sessionId = Guid.NewGuid().ToString();
        
        // Crear la instancia del driver según el tipo de navegador solicitado
        IWebDriver driver = browserType.ToLower() switch
        {
            "chrome" => CreateChromeDriver(headless),
            "firefox" => CreateFirefoxDriver(headless),
            _ => throw new ArgumentException($"Browser type '{browserType}' not supported")
        };

        // Almacenar el driver en el diccionario de sesiones activas
        _drivers[sessionId] = driver;
        
        // Establecer esta sesión como la actual
        _currentSessionId = sessionId;
        
        return sessionId;
    }

    /// <summary>
    /// Obtiene el driver de la sesión actualmente activa.
    /// </summary>
    /// <returns>Instancia de IWebDriver de la sesión actual</returns>
    /// <exception cref="InvalidOperationException">Se lanza cuando no hay sesión activa</exception>
    public IWebDriver GetCurrentDriver()
    {
        if (_currentSessionId == null || !_drivers.TryGetValue(_currentSessionId, out var driver))
        {
            throw new InvalidOperationException("No active browser session. Please start a browser first.");
        }
        return driver;
    }

    /// <summary>
    /// Verifica si hay una sesión de navegador activa.
    /// </summary>
    /// <returns>True si hay una sesión activa, False en caso contrario</returns>
    public bool HasActiveSession()
    {
        return _currentSessionId != null && _drivers.ContainsKey(_currentSessionId);
    }

    /// <summary>
    /// Cierra una sesión específica de navegador o la sesión actual si no se especifica.
    /// </summary>
    /// <param name="sessionId">ID de la sesión a cerrar. Si es null, cierra la sesión actual</param>
    public void CloseSession(string? sessionId = null)
    {
        // Determinar qué sesión cerrar (la especificada o la actual)
        var targetSessionId = sessionId ?? _currentSessionId;
        
        // Intentar remover y cerrar la sesión
        if (targetSessionId != null && _drivers.TryRemove(targetSessionId, out var driver))
        {
            driver.Quit(); // Cierra la ventana del navegador y termina el proceso
            
            // Si cerramos la sesión actual, limpiar la referencia
            if (targetSessionId == _currentSessionId)
            {
                _currentSessionId = null;
            }
        }
    }    /// <summary>
    /// Crea y configura una instancia de ChromeDriver con opciones optimizadas.
    /// La configuración incluye supresión de logs, ventana maximizada y
    /// opciones de estabilidad para automatización.
    /// </summary>
    /// <param name="headless">Si true, ejecuta Chrome sin interfaz gráfica</param>
    /// <returns>Instancia configurada de ChromeDriver</returns>
    private static ChromeDriver CreateChromeDriver(bool headless)
    {
        var options = new ChromeOptions();
        
        // Detectar automáticamente la ubicación de Chrome
        var chromePath = FindChromeExecutable();
        if (!string.IsNullOrEmpty(chromePath))
        {
            options.BinaryLocation = chromePath;
        }
        // Si no se encuentra Chrome, Selenium intentará detectarlo automáticamente
        
        if (headless)
        {
            // Modo sin interfaz gráfica para ejecución en servidores o pruebas automáticas
            options.AddArgument("--headless");
        }
        else
        {
            // Configuraciones para hacer la ventana más visible y funcional en modo visual
            options.AddArgument("--start-maximized");           // Iniciar maximizado
            options.AddArgument("--disable-web-security");      // Deshabilitar restricciones CORS
            options.AddArgument("--disable-features=VizDisplayCompositor"); // Mejorar compatibilidad
        }
        
        // Opciones comunes para estabilidad y rendimiento
        options.AddArgument("--no-sandbox");                    // Evitar problemas de permisos
        options.AddArgument("--disable-dev-shm-usage");         // Evitar problemas de memoria compartida
        options.AddArgument("--disable-blink-features=AutomationControlled"); // Ocultar detección de automatización
        
        // Suprimir logs y mensajes innecesarios de Chrome
        options.AddArgument("--silent");                        // Modo silencioso
        options.AddArgument("--log-level=3");                   // Solo errores críticos
        options.AddArgument("--disable-logging");               // Deshabilitar logging general
        options.AddArgument("--disable-gpu-logging");           // Deshabilitar logs de GPU
        
        // Desactivar la barra de automatización para una experiencia más limpia
        options.AddExcludedArgument("enable-automation");       // Ocultar "Chrome is being controlled..."
        options.AddAdditionalOption("useAutomationExtension", false); // Deshabilitar extensión de automatización
        
        // Configurar el servicio de ChromeDriver con supresión de logs
        var service = ChromeDriverService.CreateDefaultService();
        service.SuppressInitialDiagnosticInformation = true;    // Ocultar información de diagnóstico
        service.HideCommandPromptWindow = true;                 // Ocultar ventana de comando
        
        return new ChromeDriver(service, options);
    }

    /// <summary>
    /// Busca automáticamente la ubicación del ejecutable de Chrome en el sistema.
    /// Prueba las ubicaciones más comunes en Windows y variables de entorno.
    /// </summary>
    /// <returns>Ruta completa al ejecutable de Chrome, o null si no se encuentra</returns>
    private static string? FindChromeExecutable()
    {
        // Primero verificar si hay una variable de entorno definida
        var chromeFromEnv = Environment.GetEnvironmentVariable("CHROME_BIN");
        if (!string.IsNullOrEmpty(chromeFromEnv) && File.Exists(chromeFromEnv))
        {
            return chromeFromEnv;
        }

        // Lista de ubicaciones comunes donde puede estar instalado Chrome
        var possiblePaths = new[]
        {
            // Instalación por defecto (64-bit)
            @"C:\Program Files\Google\Chrome\Application\chrome.exe",
            
            // Instalación 32-bit en sistema 64-bit
            @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe",
            
            // Instalación por usuario
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), 
                        @"Google\Chrome\Application\chrome.exe"),
            
            // Ubicación específica del usuario actual
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                        @"AppData\Local\Google\Chrome\Application\chrome.exe")
        };

        // Buscar Chrome en cada ubicación posible
        foreach (var path in possiblePaths)
        {
            if (File.Exists(path))
            {
                return path;
            }
        }

        // Si no se encuentra en ubicaciones estándar, intentar desde el registro de Windows
        try
        {
            var chromeFromRegistry = GetChromePathFromRegistry();
            if (!string.IsNullOrEmpty(chromeFromRegistry) && File.Exists(chromeFromRegistry))
            {
                return chromeFromRegistry;
            }
        }
        catch
        {
            // Ignorar errores de acceso al registro
        }

        return null; // No se encontró Chrome
    }

    /// <summary>
    /// Intenta obtener la ruta de Chrome desde el registro de Windows.
    /// </summary>
    /// <returns>Ruta de Chrome desde el registro, o null si no se encuentra</returns>
    private static string? GetChromePathFromRegistry()
    {
        // Solo funciona en Windows
        if (!OperatingSystem.IsWindows())
        {
            return null;
        }

        try
        {
            // Buscar en el registro de Windows
            using var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\chrome.exe");
            
            return key?.GetValue("")?.ToString();
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Crea y configura una instancia de FirefoxDriver con opciones básicas.
    /// </summary>
    /// <param name="headless">Si true, ejecuta Firefox sin interfaz gráfica</param>
    /// <returns>Instancia configurada de FirefoxDriver</returns>
    private static FirefoxDriver CreateFirefoxDriver(bool headless)
    {
        var options = new FirefoxOptions();
        
        if (headless)
        {
            // Configurar Firefox para ejecutar sin interfaz gráfica
            options.AddArgument("--headless");
        }
        
        return new FirefoxDriver(options);
    }

    /// <summary>
    /// Libera todos los recursos utilizados por el servicio.
    /// Cierra todas las sesiones de navegador activas de forma segura.
    /// Este método es llamado automáticamente cuando el servicio se elimina.
    /// </summary>
    public void Dispose()
    {
        // Iterar sobre todas las sesiones activas y cerrarlas de forma segura
        foreach (var driver in _drivers.Values)
        {
            try
            {
                driver.Quit(); // Cerrar el navegador y terminar el proceso
            }
            catch
            {
                // Ignorar errores durante la limpieza para evitar excepciones
                // en el proceso de finalización
            }
        }
        
        // Limpiar el diccionario de sesiones
        _drivers.Clear();
    }
}