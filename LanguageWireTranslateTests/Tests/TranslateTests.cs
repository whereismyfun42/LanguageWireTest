using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using LanguageWireTranslateTests.Drivers;
using LanguageWireTranslateTests.Pages;
using System;
using SeleniumExtras.WaitHelpers;

namespace LanguageWireTranslateTests.Tests
{
    [TestFixture]
    public class TranslateTests
    {
        private IWebDriver _driver;
        private TranslatePage _translatePage;
        private WebDriverWait _wait;
        private const string FileDownloadUrl = "http://192.168.50.221:54111/$/P4rfg"; //place where the file for tests is hosted

        [SetUp]
        public void SetUp()
        {
            var webDriverManager = new WebDriverManager();
            _driver = webDriverManager.GetWebDriver();
            _driver.Navigate().GoToUrl("https://demo-qa-interview-lwt.staging.lw-ml.net?token=UCehEUtaS3rx4mBkxrHVoLgzu%2BandA3ZSKrUmIyvoE4%3D.eyJOb25jZSI6ICJOb25jZSIsICJVc2VySWQiOiAxLCAiRGVmYXVsdENvbXBhbnlJZCI6IDEsICJMd3RTdWJzY3JpcHRpb25JZCI6IDEsICJQZXJtaXNzaW9ucyI6IFszMDkyXSwgIkV4cGlyYXRpb25UaW1lIjogIi9EYXRlKDE3MTcyMjAyODkwNTYpLyJ9");

            // Initialize WebDriverWait
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            // Wait until the page is fully loaded
            _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("button[title='Detect language']")));

            _translatePage = new TranslatePage(_driver);

            // Adding an initial delay to ensure everything is fully loaded
            System.Threading.Thread.Sleep(5000);
        }

        [Test]
        public void CopyAndPasteTranslation()
        {
            const string originalText = "“In a dark place we find ourselves and a little more knowledge lights our way.” —Yoda, Star Wars Episode III: Revenge Of The Sith";

            // Ensure the source language is set to "Detect language"
            _translatePage.EnsureSourceLanguageIsDetectLanguage();

            // Adding delay to observe the interaction
            System.Threading.Thread.Sleep(2000);

            // Set the target language to Danish
            _translatePage.SetTargetLanguageToDanish();

            // Adding delay to observe the interaction
            System.Threading.Thread.Sleep(2000);

            // Enter text into the source text area
            _translatePage.EnterText(originalText);

            // Adding delay to observe the interaction
            System.Threading.Thread.Sleep(5000);

            // Wait for the translation to appear and copy it
            string translatedText = _translatePage.GetTranslatedText();
            _translatePage.CopyTranslation();

            // Adding delay to observe the interaction
            System.Threading.Thread.Sleep(2000);

            // Change the target language to English (US)
            _translatePage.SetTargetLanguageToEnglishUS();

            // Adding delay to observe the interaction
            System.Threading.Thread.Sleep(2000);

            // Paste the translated text back into the source text area
            _translatePage.PasteTextIntoSource();

            // Adding delay to observe the interaction
            System.Threading.Thread.Sleep(8000);

            // Verify the re-translated text matches the original text
            string reTranslatedText = _translatePage.GetTranslatedText();
            TestContext.WriteLine($"Original Text: {originalText}");
            TestContext.WriteLine($"Translated Text: {translatedText}");
            TestContext.WriteLine($"Re-Translated Text: {reTranslatedText}");
            Assert.AreEqual(originalText, reTranslatedText);
        }

        [Test]
        public void LanguageSwapTest()
        {
            const string originalText = "“In a dark place we find ourselves and a little more knowledge lights our way.” —Yoda, Star Wars Episode III: Revenge Of The Sith";

            // Ensure the source language is set to "Detect language"
            _translatePage.EnsureSourceLanguageIsDetectLanguage();

            // Adding delay to observe the interaction
            System.Threading.Thread.Sleep(2000);

            // Set the target language to Danish
            _translatePage.SetTargetLanguageToDanish();

            // Adding delay to observe the interaction
            System.Threading.Thread.Sleep(2000);

            // Enter text into the source text area
            _translatePage.EnterText(originalText);

            // Adding delay to observe the interaction
            System.Threading.Thread.Sleep(5000);

            // Perform the language swap
            _translatePage.SwapLanguages();

            // Adding delay to observe the interaction
            System.Threading.Thread.Sleep(2000);

            // Check if the swap button is enabled (should be disabled if the languages are the same)
            bool isSwapButtonEnabled = _translatePage.IsLanguageSwapButtonEnabled();
            TestContext.WriteLine($"Is Swap Button Enabled: {isSwapButtonEnabled}");

            // Check the tooltip text for the swap button
            string swapTooltip = _translatePage.GetLanguageSwapTooltip();
            TestContext.WriteLine($"Swap Button Tooltip: {swapTooltip}");

            // Verify the languages are swapped correctly
            string detectedSourceLanguage = _translatePage.GetDetectedSourceLanguage();
            string detectedTargetLanguage = _translatePage.GetDetectedTargetLanguage();
            TestContext.WriteLine($"Source Language after Swap: {detectedSourceLanguage}");
            TestContext.WriteLine($"Target Language after Swap: {detectedTargetLanguage}");
            Assert.AreEqual("Danish", detectedSourceLanguage);
            Assert.AreEqual("Detect language", detectedTargetLanguage);

            // Swap back to original configuration
            _translatePage.SwapLanguages();

            // Adding delay to observe the interaction
            System.Threading.Thread.Sleep(2000);

            detectedSourceLanguage = _translatePage.GetDetectedSourceLanguage();
            detectedTargetLanguage = _translatePage.GetDetectedTargetLanguage();
            TestContext.WriteLine($"Source Language after Swap Back: {detectedSourceLanguage}");
            TestContext.WriteLine($"Target Language after Swap Back: {detectedTargetLanguage}");
            Assert.AreEqual("Detect language", detectedSourceLanguage);
            Assert.AreEqual("Danish", detectedTargetLanguage);
        }

         [Test]
        public void RandomLanguageSelection()
        {
            // Select random source and target languages
            _translatePage.SelectRandomSourceLanguage();
            _translatePage.SelectRandomTargetLanguage();

            // Store the selected languages
            string selectedSourceLanguage = _translatePage.GetDetectedSourceLanguage();
            string selectedTargetLanguage = _translatePage.GetDetectedTargetLanguage();
            TestContext.WriteLine($"Selected Source Language: {selectedSourceLanguage}");
            TestContext.WriteLine($"Selected Target Language: {selectedTargetLanguage}");

            // Reload the page
            _driver.Navigate().Refresh();

            // Wait until the page is fully loaded
            _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("button[title='Detect language']")));

            // Verify the selected languages remain the same after reload
            string reloadedSourceLanguage = _translatePage.GetDetectedSourceLanguage();
            string reloadedTargetLanguage = _translatePage.GetDetectedTargetLanguage();
            TestContext.WriteLine($"Reloaded Source Language: {reloadedSourceLanguage}");
            TestContext.WriteLine($"Reloaded Target Language: {reloadedTargetLanguage}");
            Assert.AreEqual(selectedSourceLanguage, reloadedSourceLanguage);
            Assert.AreEqual(selectedTargetLanguage, reloadedTargetLanguage);
        }

        [Test]
        public void UploadDocumentTest()
        {
            // Ensure the source language is set to "Detect language"
            _translatePage.EnsureSourceLanguageIsDetectLanguage();

            // Adding delay to observe the interaction
            System.Threading.Thread.Sleep(2000);

            // Set the target language to Danish
            _translatePage.SetTargetLanguageToDanish();

            // Adding delay to observe the interaction
            System.Threading.Thread.Sleep(2000);

            // Download the document
            _translatePage.DownloadFile(FileDownloadUrl);

            // Adding delay to ensure the file is downloaded
            System.Threading.Thread.Sleep(5000);

            // Upload the document
            _translatePage.UploadDocument();

            // Adding delay to observe the interaction
            System.Threading.Thread.Sleep(2000);

            // Translate the document
            _translatePage.TranslateDocument();

            // Wait for the document to be translated and the download popup to appear
            System.Threading.Thread.Sleep(20000);

            // Save the translated document to the first available folder
            _translatePage.WaitForDownloadPopup();
        }

        [TearDown]
        public void TearDown()
        {
            _driver.Quit();
        }
    }
}
