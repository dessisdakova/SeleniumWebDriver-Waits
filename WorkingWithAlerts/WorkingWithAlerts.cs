using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.DevTools;

namespace WorkingWithAlerts
{
    public class WorkingWithAlerts
    {
        IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--window-size=1920,1080");
            options.AddArgument("--disable-infobars");
            driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/javascript_alerts");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();
            driver.Dispose();
        }

        [Test]
        public void HandlingBasicJavaScriptAlerts()
        {
            driver.FindElement(By.CssSelector("button[onclick='jsAlert()']")).Click();
            IAlert alert = driver.SwitchTo().Alert();
            Assert.That(alert.Text, Is.EqualTo("I am a JS Alert"));
            alert.Accept();
            var result = driver.FindElement(By.Id("result"));
            Assert.That(result.Text, Is.EqualTo("You successfully clicked an alert"));
        }

        [Test]
        public void HandlingJavaScriptConfirmAlerts_Confirm()
        {
            driver.FindElement(By.CssSelector("button[onclick='jsConfirm()']")).Click();

            var alert = driver.SwitchTo().Alert();
            Assert.That(alert.Text, Is.EqualTo("I am a JS Confirm"));

            alert.Accept();
            var result = driver.FindElement(By.Id("result"));
            Assert.That(result.Text, Is.EqualTo("You clicked: Ok"));

            driver.FindElement(By.CssSelector("button[onclick='jsConfirm()']")).Click();

            alert = driver.SwitchTo().Alert();
            Assert.That(alert.Text, Is.EqualTo("I am a JS Confirm"));

            alert.Dismiss();
            result = driver.FindElement(By.Id("result"));
            Assert.That(result.Text, Is.EqualTo("You clicked: Cancel"));
        }

        [Test]
        public void HandlingJavaScriptPromptAlerts()
        {
            driver.FindElement(By.CssSelector("button[onclick='jsPrompt()']")).Click();

            var alert = driver.SwitchTo().Alert();
            Assert.That(alert.Text, Is.EqualTo("I am a JS prompt"));

            alert.SendKeys("Desislava");
            alert.Accept();
            var result = driver.FindElement(By.Id("result"));
            Assert.That(result.Text, Is.EqualTo("You entered: Desislava"));

            driver.FindElement(By.CssSelector("button[onclick='jsPrompt()']")).Click();
            alert = driver.SwitchTo().Alert();
            alert.Accept();
            result = driver.FindElement(By.Id("result"));
            Assert.That(result.Text, Is.EqualTo("You entered:"));

            driver.FindElement(By.CssSelector("button[onclick='jsPrompt()']")).Click();
            alert = driver.SwitchTo().Alert();
            alert.Dismiss();
            result = driver.FindElement(By.Id("result"));
            Assert.That(result.Text, Is.EqualTo("You entered: null"));
        }
    }
}