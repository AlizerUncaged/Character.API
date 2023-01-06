using PuppeteerSharp;

namespace Character.API.Browser;

public class HeadlessBrowserHandler
{
    private readonly BrowserOptions _options;
    private IBrowser? _browser;
    private IBrowserContext? incognito;

    public BrowserFetcher Fetcher { get; } = new BrowserFetcher();

    public HeadlessBrowserHandler(BrowserOptions? options = null)
    {
        _options = options ?? new BrowserOptions();
    }


    /// <summary>
    /// Starts downloading the headless browser to be used.
    /// </summary>
    /// <returns>This instance of CharacterAi object.</returns>
    public async Task InitializeConnectionAsync()
    {
        // Download Chrome.
        await Fetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);

        _browser = await Puppeteer.LaunchAsync(new LaunchOptions()
        {
            Headless = false,
            DefaultViewport = new ViewPortOptions()
            {
                Width = 500, Height = 400
            }
        });

        incognito = await _browser.CreateIncognitoBrowserContextAsync();
    }

    public async Task<string> SendAsync(string url, string body, string contentType = "application/json",
        HttpMethod? method = null, string? authorization = null)
    {
        var page = await incognito?.NewPageAsync()!;

        await page.SetRequestInterceptionAsync(true);

        page.Request += async (s, e) =>
        {
            var payload = new Payload
            {
                Url = url,
                Method = method ?? HttpMethod.Post,
                PostData = body,
                Headers = e.Request.Headers,
            };

            payload.Headers["User-Agent"] = _options.UserAgent;
            payload.Headers["Content-Type"] = contentType;

            if (!string.IsNullOrWhiteSpace(authorization))
                payload.Headers["Authorization"] = authorization;

            await e.Request.ContinueAsync(payload);
        };

        var response = await page.GoToAsync(url);
        var responseText = await response.TextAsync();

        return responseText;
    }
}