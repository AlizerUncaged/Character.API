using System.Drawing;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using PuppeteerSharp;
using Selenium.Extensions;
using SeleniumUndetectedChromeDriver;

namespace Character.API.Browser;

public class HeadlessBrowser
{
    private readonly BrowserOptions _options;

    private UndetectedChromeDriver _undetectedChromeDriver;

    public BrowserFetcher Fetcher { get; } = new BrowserFetcher();

    public HeadlessBrowser(BrowserOptions? options = null)
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
        var browserFetcher = new BrowserFetcher(Product.Chrome);
        var revisionInfo = await browserFetcher.DownloadAsync();
        // await Fetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);

        var options = new ChromeOptions();
        options.AddArguments(
            "--no-first-run",
            "--no-default-browser-check",
            "--disable-features=ChromeWhatsNewUI",
            "--disable-blink-features=AutomationControlled",
            "--remote-debugging-port=9222",
            "--disable-dev-shm-usage",
            "--disable-gpu",
            "--no-sandbox",
            "--ignore-certificate-errors",
            "--disable-setuid-sandbox",
            "--disable-infobars",
            "--lang=it",
            "--no-service-autorun",
            "--no-zygote",
            "--mute-audio",
            "--disable-accelerated-2d-canvas",
            "--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/109.0.0.0 Safari/537.3");

        // var browserOptions = 
        options.BinaryLocation = @".local-chromium\Win64-970485\chrome-win\chrome.exe";

        _undetectedChromeDriver =
            UndetectedChromeDriver.Create(
                browserExecutablePath: revisionInfo.ExecutablePath,
                options: options,
                driverExecutablePath:
                @".local-chromium\Win64-970485\chrome-win\chromedriver.exe"
                // , headless: true, suppressWelcome: true
            );

        _undetectedChromeDriver.Manage().Window.Size = new Size(800, 1080);


        var pluginGenerator =
            "{\n\t\tvar ChromiumPDFPlugin = {};\n\t\tChromiumPDFPlugin.__proto__ = Plugin.prototype;\n\t\tvar plugins = {\n\t\t\t0: ChromiumPDFPlugin,\n\t\t\tdescription: 'Portable Document Format',\n\t\t\tfilename: 'internal-pdf-viewer',\n\t\t\tlength: 1,\n\t\t\tname: 'Chromium PDF Plugin',\n\t\t\t__proto__: PluginArray.prototype,\n\t\t};\n\t\treturn plugins;\n\t}";

        _undetectedChromeDriver.ExecuteCdpCommand("Page.addScriptToEvaluateOnNewDocument",
            new Dictionary<string, object>()
            {
                {
                    "source", @"
                                     Object.defineProperty(navigator, 'plugins', {
                                           get: () => " + pluginGenerator + @"

                                         });
                                     Object.defineProperty(navigator, 'mimeTypes', {
                                           get: () => " + pluginGenerator + @"

                                         });

                                     Object.defineProperty(navigator, 'pdfViewerEnabled', {
                                           get: () => true
                                         });

                                     Object.defineProperty(navigator, 'connection', {
                                           get: () => {rtt: 0, downlink: 6.5}

                                         });

                                        const getParameter = WebGLRenderingContext.getParameter;
                                        WebGLRenderingContext.prototype.getParameter = function(parameter) {
                                          // UNMASKED_VENDOR_WEBGL
                                          if (parameter === 37445) {
                                            return 'Intel Open Source Technology Center';
                                          }
                                          // UNMASKED_RENDERER_WEBGL
                                          if (parameter === 37446) {
                                            return 'Mesa DRI Intel(R) Ivybridge Mobile ';
                                          }

                                          return getParameter(parameter);
                                        };

                                        ['height', 'width'].forEach(property => {
                                          // store the existing descriptor
                                          const imageDescriptor = Object.getOwnPropertyDescriptor(HTMLImageElement.prototype, property);

                                          // redefine the property with a patched descriptor
                                          Object.defineProperty(HTMLImageElement.prototype, property, {
                                            ...imageDescriptor,
                                            get: function() {
                                              // return an arbitrary non-zero dimension if the image failed to load
                                              if (this.complete && this.naturalHeight == 0) {
                                                return 20;
                                              }
                                              // otherwise, return the actual dimension
                                              return imageDescriptor.get.apply(this);
                                            },
                                          });
                                        });

                                        // store the existing descriptor
                                        const elementDescriptor = Object.getOwnPropertyDescriptor(HTMLElement.prototype, 'offsetHeight');

                                        // redefine the property with a patched descriptor
                                        Object.defineProperty(HTMLDivElement.prototype, 'offsetHeight', {
                                          ...elementDescriptor,
                                          get: function() {
                                            if (this.id === 'modernizr') {
                                                return 1;
                                            }
                                            return elementDescriptor.get.apply(this);
                                          },
                                        });
"
                },
            }
        );
    }

    void NewPage()
    {
    }

    async Task WaitUntilPageLoads()
    {
        await Task.Run(() =>
        {
            var wait = new WebDriverWait(_undetectedChromeDriver, TimeSpan.FromSeconds(10));
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
        });
    }

    public async Task<string> SendAsync(string url, string body, string contentType = "application/json",
        HttpMethod? method = null, string? authorization = null)
    {
        method ??= HttpMethod.Post;

        IJavaScriptExecutor jsExecutor = _undetectedChromeDriver;


        if (method == HttpMethod.Get)
        {
            _undetectedChromeDriver.GoToUrl(url);

            var actions = new Actions(_undetectedChromeDriver);
            actions.MoveByOffset(100, 100);
            actions.Pause(TimeSpan.FromMilliseconds(10));
            actions.ScrollByAmount(0, 200);
            actions.Perform();

            await WaitUntilPageLoads();

//             Console.WriteLine("entire navigator class: " +
//                               jsExecutor.ExecuteScript(@"function recur(obj) {
//   var result = {}, _tmp;
//   for (var i in obj) {
//     // enabledPlugin is too nested, also skip functions
//     if (i === 'enabledPlugin' || typeof obj[i] === 'function') {
//         continue;
//     } else if (typeof obj[i] === 'object') {
//         // get props recursively
//         _tmp = recur(obj[i]);
//         // if object is not {}
//         if (Object.keys(_tmp).length) {
//             result[i] = _tmp;
//         }
//     } else {
//         // string, number or boolean
//         result[i] = obj[i];
//     }
//   }
//   return result;
// }
//
// return JSON.stringify(recur(window.navigator))
//
// ").ToString());
//             
            // Console.WriteLine("navigator.pdfViewerEnabled: " +
            //                   jsExecutor.ExecuteScript("return navigator.pdfViewerEnabled").ToString());
            //
            // Console.WriteLine("window.outerWidth: " +
            //                   jsExecutor.ExecuteScript("return window.outerWidth").ToString());
            //
            // Console.WriteLine("navigator.userAgent: " +
            //                   jsExecutor.ExecuteScript("return navigator.userAgent").ToString());
            //
            // Console.WriteLine("navigator.webdriver: " +
            //                   jsExecutor.ExecuteScript("return navigator.webdriver"));
            //
            // Console.WriteLine("window.chrome: " +
            //                   jsExecutor.ExecuteScript("return window.chrome"));
            //
            //
            // Console.WriteLine("window.navigator.plugins: " +
            //                   jsExecutor.ExecuteScript("return JSON.stringify(window.navigator.plugins)"));
            //
            // Console.WriteLine("window.navigator.mimeTypes: " +
            //                   jsExecutor.ExecuteScript("return JSON.stringify(window.navigator.mimeTypes)"));


            string pageSource = (string)_undetectedChromeDriver.ExecuteScript("return document.body.innerText");
            return pageSource;
        }

        return "";
    }
}