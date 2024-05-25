using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using SeleniumExtras.WaitHelpers;

namespace LanguageWireTranslateTests.Pages
{
    public class TranslatePage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public TranslatePage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        // Page elements
        private IWebElement DetectLanguageButton => _driver.FindElement(By.CssSelector("button[title='Detect language']"));
        private IWebElement SourceLanguageDropdownButton => _driver.FindElement(By.XPath("/html/body/lw-languagewire-translate/lw-home/div/div/lw-language-select-bar/div/lw-language-select[1]/div/button[5]/span[4]"));
        private IWebElement TargetLanguageDropdownButton => _driver.FindElement(By.XPath("/html/body/lw-languagewire-translate/lw-home/div/div/lw-language-select-bar/div/lw-language-select[2]/div/button[4]/span[4]"));
        private IWebElement DanishLanguageOption => _driver.FindElement(By.XPath("//div[@class='cdk-overlay-container']//span[text()='Danish']"));
        private IWebElement SourceTextArea => _driver.FindElement(By.CssSelector("textarea[name='translation-input']"));
        private IWebElement TranslationOutput => _driver.FindElement(By.CssSelector("section.lw-translation__output"));
        private IWebElement CopyTranslationButton => _driver.FindElement(By.CssSelector("button.mat-mdc-tooltip-trigger.lw-output-text__copy-button"));
        private IWebElement LanguageSwapButton => _driver.FindElement(By.XPath("/html/body/lw-languagewire-translate/lw-home/div/div/lw-language-select-bar/div/div"));
        private IWebElement UploadDocumentButton => _driver.FindElement(By.XPath("/html/body/lw-languagewire-translate/lw-home/div/div/lw-translation/div/section[1]/lw-translation-input/lw-translation-placeholder/div/button"));
        private IWebElement TranslateDocumentButton => _driver.FindElement(By.XPath("/html/body/lw-languagewire-translate/lw-home/div/div/lw-translation/div/section[1]/lw-source-document/div[2]/button"));

        // Page interactions
        public void EnsureSourceLanguageIsDetectLanguage()
        {
            if (!DetectLanguageButton.Displayed)
            {
                SourceLanguageDropdownButton.Click();
                _wait.Until(ExpectedConditions.ElementToBeClickable(DetectLanguageButton)).Click();
            }
        }

        public void SetTargetLanguageToDanish()
        {
            try
            {
                var danishButton = _driver.FindElement(By.XPath("//div[@class='cdk-overlay-container']//span[text()='Danish']"));
                if (!danishButton.Displayed)
                {
                    TargetLanguageDropdownButton.Click();
                    _wait.Until(ExpectedConditions.ElementToBeClickable(DanishLanguageOption)).Click();
                }
            }
            catch (NoSuchElementException)
            {
                TargetLanguageDropdownButton.Click();
                _wait.Until(ExpectedConditions.ElementToBeClickable(DanishLanguageOption)).Click();
            }
        }

        public void SetTargetLanguageToEnglishUS()
        {
            // Click the target language dropdown button
            TargetLanguageDropdownButton.Click();

            // Wait for the dropdown to appear
            _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/div[2]/div/div/div/div[2]/div/button[1]")));

            // Find all buttons in the dropdown
            var buttons = _driver.FindElements(By.XPath("/html/body/div[2]/div/div/div/div[2]/div/button[*]"));

            // Iterate through the buttons and click the one with the text "English (US)"
            foreach (var button in buttons)
            {
                if (button.Text.Trim() == "English (US)")
                {
                    button.Click();
                    break;
                }
            }
        }

        public void EnterText(string text)
        {
            SourceTextArea.Clear();
            SourceTextArea.SendKeys(text);
        }

        public string GetTranslatedText()
        {
            _wait.Until(driver => TranslationOutput.Displayed);
            var translation = TranslationOutput.FindElement(By.CssSelector("div.lw-output-text__text")).Text;
            return translation;
        }

        public void CopyTranslation()
        {
            _wait.Until(ExpectedConditions.ElementToBeClickable(CopyTranslationButton)).Click();
        }

        public void PasteTextIntoSource()
        {
            _wait.Until(ExpectedConditions.ElementToBeClickable(SourceTextArea));
            SourceTextArea.Clear();
            SourceTextArea.SendKeys(OpenQA.Selenium.Keys.Control + "v");
        }

        public void SwapLanguages()
        {
            _wait.Until(ExpectedConditions.ElementToBeClickable(LanguageSwapButton)).Click();
        }

        public bool IsLanguageSwapButtonEnabled()
        {
            return LanguageSwapButton.Enabled;
        }

        public string GetLanguageSwapTooltip()
        {
            return LanguageSwapButton.GetAttribute("title");
        }

        public string GetDetectedSourceLanguage()
        {
            var sourceLanguageButtons = _driver.FindElements(By.XPath("/html/body/lw-languagewire-translate/lw-home/div/div/lw-language-select-bar/div/lw-language-select[1]/div/button"));
            foreach (var button in sourceLanguageButtons)
            {
                if (button.GetAttribute("class").Contains("lw-language-select__button--selected"))
                {
                    return button.Text.Trim();
                }
            }
            return string.Empty;
        }

        public string GetDetectedTargetLanguage()
        {
            var targetLanguageButtons = _driver.FindElements(By.XPath("/html/body/lw-languagewire-translate/lw-home/div/div/lw-language-select-bar/div/lw-language-select[2]/div/button"));
            foreach (var button in targetLanguageButtons)
            {
                if (button.GetAttribute("class").Contains("lw-language-select__button--selected"))
                {
                    return button.Text.Trim();
                }
            }
            return string.Empty;
        }

        public void SelectRandomSourceLanguage()
        {
            SourceLanguageDropdownButton.Click();
            var sourceLanguageButtons = _driver.FindElements(By.XPath("/html/body/div[2]/div/div/div/div[2]/div/button[*]"));
            var random = new Random();
            int index = random.Next(0, sourceLanguageButtons.Count);
            sourceLanguageButtons[index].Click();
        }

        public void SelectRandomTargetLanguage()
        {
            TargetLanguageDropdownButton.Click();
            var targetLanguageButtons = _driver.FindElements(By.XPath("/html/body/div[2]/div/div/div/div[2]/div/button[*]"));
            var random = new Random();
            int index = random.Next(0, targetLanguageButtons.Count);
            targetLanguageButtons[index].Click();
        }

        public void DownloadFile(string fileDownloadUrl)
        {
            _driver.Navigate().GoToUrl(fileDownloadUrl);
        }

        public void UploadDocument()
        {
            _wait.Until(ExpectedConditions.ElementToBeClickable(UploadDocumentButton)).Click();
            var fileUpload = _driver.FindElement(By.CssSelector("input[type='file']"));
            fileUpload.SendKeys("/home/seluser/Downloads/translatetest.docx");
        }

        public void TranslateDocument()
        {
            _wait.Until(ExpectedConditions.ElementToBeClickable(TranslateDocumentButton)).Click();
        }

        public void WaitForDownloadPopup()
        {
            _wait.Until(ExpectedConditions.ElementExists(By.XPath("//div[@class='cdk-overlay-container']")));
        }
    }
}
