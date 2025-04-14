using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;
using System;
using System.Linq;

namespace OFMC_Test
{
    public class AppointmentActionTests
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private string baseUrl = "http://localhost:5058";

        [SetUp]
        public void Setup()
        {
            // Initialize Chrome browser instance
            var options = new ChromeOptions();
            driver = new ChromeDriver(options);

            // Optional: apply an implicit wait as a fallback
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            // Create a WebDriverWait for explicit waits
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// Logs in using test patient credentials.
        /// Navigates to login form, fills in fields, and clicks the login button.
        /// </summary>
        private void LoginAsPatient()
        {
            driver.Navigate().GoToUrl(baseUrl + "/Login/Patient/Form");

            // Wait until the email input is present before proceeding
            wait.Until(d => d.FindElements(By.Id("Email")).Count > 0);

            // Fill in login fields
            driver.FindElement(By.Id("Email")).SendKeys("hang@gmail.com");
            driver.FindElement(By.Id("Password")).SendKeys("Test@1234");

            // Click the login button
            var loginBtn = driver.FindElement(By.XPath("//button[contains(text(), 'Log In')]"));
            ScrollAndClick(loginBtn);

            // Wait until redirected to the appointments dashboard
            wait.Until(d => d.Url.Contains("/appointments"));
        }

        /// <summary>
        /// Test Case 1:
        /// Reschedules the first available appointment by selecting a different time slot.
        /// Verifies redirection to appointments page after success.
        /// </summary>
        [Test, Order(1)]
        public void RescheduleAppointment_ShouldRedirectToAppointments()
        {
            // Step 1: Log in with test patient credentials
            LoginAsPatient();

            // Step 2: Locate the first "Reschedule" link
            var rescheduleLink = wait.Until(d =>
                d.FindElements(By.LinkText("Reschedule")).FirstOrDefault());
            Assert.That(rescheduleLink, Is.Not.Null, "Reschedule link not found. Ensure appointments exist.");
            ScrollAndClick(rescheduleLink);

            // Step 3: Wait for the reschedule form to load
            wait.Until(d => d.Url.Contains("/reschedule-appointment-form"));

            // Step 4: Select a different available time slot
            var slotDropdown = new SelectElement(driver.FindElement(By.Name("ActiveAppointment.AppointmentDate")));
            Assert.That(slotDropdown.Options.Count, Is.GreaterThan(1), "Not enough available slots.");
            slotDropdown.SelectByIndex(slotDropdown.Options.Count - 1); // Select last one for test

            // Step 5: Submit the reschedule form
            var saveBtn = driver.FindElement(By.CssSelector("button[type='submit']"));
            ScrollAndClick(saveBtn);

            // Step 6: Assert redirection back to appointments
            wait.Until(d => d.Url.Contains("/appointments"));
            Assert.That(driver.Url, Does.Contain("/appointments"), "Reschedule did not redirect as expected.");
        }

        /// <summary>
        /// Scrolls an element into view before clicking.
        /// Prevents errors when elements are out of viewport.
        /// </summary>
        private void ScrollAndClick(IWebElement element)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
            element.Click();
        }

        [TearDown]
        public void TearDown()
        {
            // Clean up browser after test
            driver.Quit();
            driver.Dispose();   
        }
    }
}
