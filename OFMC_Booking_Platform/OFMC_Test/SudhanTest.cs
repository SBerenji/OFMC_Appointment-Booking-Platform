using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace OFMC_Test
{
    public class SudhanTest
    {
        private IWebDriver driver;
        private string baseUrl = "https://localhost:7276";

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
        }


        [Test]
        public void Register_NewPatient_ShouldRedirectToAppointments()
        {
            // -------------------- Arrange --------------------
            // Here we navigate to the registration page.
            driver.Navigate().GoToUrl(baseUrl + "/Patient/Register");

            // Here we fill out the registration form fields.
            driver.FindElement(By.Name("FirstName")).SendKeys("John");
            driver.FindElement(By.Name("LastName")).SendKeys("Doe");
            driver.FindElement(By.Name("DateOfBirth")).SendKeys("2000-01-01");
            driver.FindElement(By.Name("PhoneNumber")).SendKeys("2345678904");

            var email = $"johndoe_{DateTime.Now.Ticks}@mail.com";
            driver.FindElement(By.Name("Email")).SendKeys(email);
            driver.FindElement(By.Name("Password")).SendKeys("Test@1234");
            driver.FindElement(By.Name("ConfirmPassword")).SendKeys("Test@1234");




            // -------------------- Act --------------------
            // Here we find the register form by action (this is because we also have Login Button.) and get the Register button.
            var registerForm = driver.FindElement(By.CssSelector("form[action='/Register/Patient']"));

            var registerButton = registerForm.FindElement(By.CssSelector("button[type='submit']"));

            // We click the Register button.
            registerButton.Click();

            // We wait for 10 seconds to get redirected to appointments page.
            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(d => d.Url.Contains("/appointments"));


            // -------------------- Assert --------------------
            // We verify the URL contains /appointments.
            // This means we have successfully registered.
            Assert.That(driver.Url, Does.Contain("/appointments"));
        }


        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}
