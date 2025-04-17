using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;
using System;
using System.Linq;

namespace OFMC_Test
{
    public class HangTest
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private string baseUrl = "http://localhost:5058";

        [SetUp]
        public void Setup()
        {
            // Arrange: Set up browser and wait settings
            driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// Logs in as a test patient (assumes the account already exists and is seeded/registered).
        /// </summary>
        private void LoginAsPatient()
        {
            // Arrange: Navigate to patient login form
            driver.Navigate().GoToUrl(baseUrl + "/Login/Patient/Form");

            // Wait for email input field to ensure the form is fully loaded
            wait.Until(d => d.FindElement(By.Id("Email")));

            // Act: Fill login credentials
            driver.FindElement(By.Id("Email")).SendKeys("serbe25@gmail.com");
            driver.FindElement(By.Id("Password")).SendKeys("NewPassword25!");

            // Act: Click the login button
            var loginBtn = driver.FindElement(By.XPath("//button[contains(text(), 'Log In')]"));
            ScrollAndClick(loginBtn);

            // Assert: Wait until redirected to appointments dashboard
            wait.Until(d => d.Url.Contains("/appointments"));
        }

        /// <summary>
        /// Test Case 1: Reschedules an existing appointment by selecting a different time slot.
        /// </summary>
        [Test, Order(1)]
        public void RescheduleAppointment_ShouldSucceed()
        {
            // ========== Arrange ==========
            LoginAsPatient(); // Log in as a test patient

            // Find the first "Reschedule" link
            var rescheduleLink = wait.Until(d => d.FindElements(By.LinkText("Reschedule")).FirstOrDefault());
            Assert.That(rescheduleLink, Is.Not.Null, "No reschedulable appointments found.");
            ScrollAndClick(rescheduleLink); // Go to reschedule form

            // ========== Act ==========
            wait.Until(d => d.Url.Contains("/reschedule-appointment-form")); // Ensure we're on the form page

            // Find the available time slot dropdown
            var timeSlotDropdown = new SelectElement(driver.FindElement(By.Name("ActiveAppointment.AppointmentDate")));
            Assert.That(timeSlotDropdown.Options.Count, Is.GreaterThan(1), "Not enough available slots to reschedule.");

            // Select the last slot for demonstration purposes
            timeSlotDropdown.SelectByIndex(timeSlotDropdown.Options.Count - 1);

            // Submit the reschedule form
            var saveButton = driver.FindElement(By.CssSelector("button[type='submit']"));
            ScrollAndClick(saveButton);

            // ========== Assert ==========
            // Wait for redirection back to appointment list
            wait.Until(d => d.Url.Contains("/appointments"));
            Assert.That(driver.Url, Does.Contain("/appointments"), "Redirection to appointments failed after rescheduling.");
        }

        /// <summary>
        /// Test Case 2: Cancels an existing appointment and verifies user is redirected and receives confirmation.
        /// </summary>
        [Test, Order(2)]
        public void CancelAppointment_ShouldSucceed()
        {
            // ========== Arrange ==========
            LoginAsPatient();

            // Get all unique appointment detail (e.g., doctor name or datetime) for the first appointment
            var initialRow = driver.FindElement(By.CssSelector("table tbody tr"));
            var appointmentIdentifier = initialRow.FindElements(By.TagName("td"))[0].Text.Trim(); // Date & Time cell

            // Ensure we have at least one appointment to cancel
            var cancelLink = wait.Until(d => d.FindElements(By.LinkText("Cancel")).FirstOrDefault());
            Assert.That(cancelLink, Is.Not.Null, "No cancellable appointments found.");

            // Click the "Cancel" link to go to confirmation form
            ScrollAndClick(cancelLink);

            // ========== Act ==========
            // Wait for the confirmation page to load
            wait.Until(d => d.Url.Contains("/appointment/cancel-form"));

            // Find and click the "Yes" button (confirmation)
            var yesButton = wait.Until(d => d.FindElement(By.CssSelector("form button.btn-danger")));
            ScrollAndClick(yesButton);

            // Wait for redirection to /appointments
            wait.Until(d => d.Url.Contains("/appointments"));

            // ========== Assert ==========
            // Wait and ensure the canceled appointment (by its unique date/time) is not present anymore
            bool appointmentGone = wait.Until(d =>
            {
                var rows = d.FindElements(By.CssSelector("table tbody tr"));
                return rows.All(r => !r.Text.Contains(appointmentIdentifier));
            });

            // Ensure an appointment was actually removed
            Assert.That(appointmentGone, Is.True, "Canceled appointment still appears in the list.");
        }



        /// <summary>
        /// Utility: Scrolls to an element and performs a click using JavaScript
        /// </summary>
        private void ScrollAndClick(IWebElement element)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
            element.Click();
        }

        [TearDown]
        public void TearDown()
        {
            // Cleanup after each test
            driver.Quit();
            driver.Dispose();
        }
    }
}
