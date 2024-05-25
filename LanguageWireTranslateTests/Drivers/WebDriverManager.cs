using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;

namespace LanguageWireTranslateTests.Drivers
{
    public class WebDriverManager
    {
        public IWebDriver GetWebDriver()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");

            var remoteWebDriverUrl = "http://192.168.50.221:4444/wd/hub"; // place where the selenium hub should be hosted 
            return new RemoteWebDriver(new Uri(remoteWebDriverUrl), options.ToCapabilities());
        }
    }
}
