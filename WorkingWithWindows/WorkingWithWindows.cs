using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace WorkingWithWindows
{
    public class WorkingWithWindows
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
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/windows");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();
            driver.Dispose();
        }

        [Test]
        public void HandlingMultipleWindows()
        {
            driver.FindElement(By.LinkText("Click Here")).Click();

            var windowHandles = driver.WindowHandles;

            Assert.That(windowHandles.Count, Is.EqualTo(2));

            driver.SwitchTo().Window(windowHandles[1]);

            string newWindowContent = driver.PageSource;
            Assert.IsTrue(newWindowContent.Contains("New Window"));

            // log the content of the new window
            string path = Path.Combine(Directory.GetCurrentDirectory() + "window.txt");
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            File.AppendAllText(path, "Window handle for new tab: " + driver.CurrentWindowHandle + "\n\n");
            File.AppendAllText(path, "The page content: " + newWindowContent + "\n\n");
            driver.Close();

            driver.SwitchTo().Window(windowHandles[0]);
            string originalWindowContent = driver.PageSource;
            Assert.IsTrue(originalWindowContent.Contains("Opening a new window"));

            // log the content of the original window
            File.AppendAllText(path, "Window handle for original window: " + driver.CurrentWindowHandle + "\n\n");
            File.AppendAllText(path, "The page content: " + originalWindowContent + "\n\n");
        }

        [Test]
        public void HandlingNoSuchWindowException()
        {
            driver.FindElement(By.LinkText("Click Here")).Click();
            var windowHandles = driver.WindowHandles;

            driver.SwitchTo().Window(windowHandles[1]);
            Assert.IsTrue(driver.PageSource.Contains("New Window"));
            driver.Close();

            try
            {
                driver.SwitchTo().Window(windowHandles[1]);
            }
            catch (NoSuchWindowException ex)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory() + "exception.txt");
                File.AppendAllText(path, "NoSuchWindowException caugth: " + ex.Message + "\n\n");
                Assert.Pass("NoSuchWindow expection was thrown");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected exception: " + ex.Message);
            }
        }
    }
}