using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SearchProductWithImplicitWait
{
    public class SearchProductWithImplicitWait
    {
        WebDriver driver;
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
            driver.Navigate().GoToUrl("http://practice.bpbonline.com/");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
            driver.Dispose();
        }

        [Test]
        public void SearchProduct_Keyboard_ShouldAddToCart()
        {
            driver.FindElement(By.XPath("//input[@name='keywords']")).SendKeys("keyboard");
            driver.FindElement(By.XPath("//input[@alt='Quick Find']")).Click();

            try
            {
                driver.FindElement(By.LinkText("Buy Now")).Click();
                Assert.IsTrue(driver.PageSource.Contains("keyboard"));
                Console.WriteLine("Scenario completed!");
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception:" + ex.Message);
            }

        }

        [Test]
        public void SearchProduct_Junk_ShouldThrowNoSuchElementException()
        {
            driver.FindElement(By.XPath("//input[@name='keywords']")).SendKeys("junk");
            driver.FindElement(By.XPath("//input[@alt='Quick Find']")).Click();

            try
            {
                driver.FindElement(By.LinkText("Buy Now")).Click();
            }
            catch (NoSuchElementException ex)
            {
                Assert.Pass("NoSuchElementException was thrown");
                Console.WriteLine("Timeout -- " + ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception:" + ex.Message);
            }
        }
    }
}