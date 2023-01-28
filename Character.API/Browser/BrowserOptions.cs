namespace Character.API.Browser;

/// <summary>
/// Class for specifying options for the headless browser.
/// </summary>
public class BrowserOptions
{
    /// <summary>
    /// The user agent to use when making requests with the headless browser.
    /// </summary>
    public string UserAgent { get; set; } =
        "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36";
}