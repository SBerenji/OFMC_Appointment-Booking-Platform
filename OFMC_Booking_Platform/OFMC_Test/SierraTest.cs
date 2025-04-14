using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Buffers.Text;

namespace OFMC_Test
{
    public class SierraTest
    {
        private IWebDriver driver;
        private string baseUrl = "http://localhost:5058";

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
        }

        private void registerPatient()
        {
            //Log in process
            //Go to the patient log in form
            driver.Navigate().GoToUrl($"{baseUrl}/Patient/Register?");

            //Find the FirsName text box
            var firstnameInput = driver.FindElement(By.Id("FirstName"));
            firstnameInput.SendKeys("Test");

            //Find the LastName text box
            var lastnameInput = driver.FindElement(By.Id("LastName"));
            lastnameInput.SendKeys("Test");

            //Find the email text box
            var dateofbirthInput = driver.FindElement(By.Id("DateOfBirth"));
            dateofbirthInput.SendKeys("1999");
            dateofbirthInput.SendKeys(Keys.Tab);
            dateofbirthInput.SendKeys("10-10");

            //Find the password text box
            var phonenumberInput = driver.FindElement(By.Name("PhoneNumber"));
            phonenumberInput.SendKeys("111-111-11111");

            //Find the LastName text box
            var emailInput = driver.FindElement(By.Id("Email"));
            emailInput.SendKeys("serbe25@gmail.com");

            //Find the email text box
            var passwordInput = driver.FindElement(By.Id("Password"));
            passwordInput.SendKeys("NewPassword25!");

            //Find the password text box
            var confirmpasswordInput = driver.FindElement(By.Id("ConfirmPassword"));
            confirmpasswordInput.SendKeys("NewPassword25!");


            //Find the log in button
            var registerButton = driver.FindElement(By.XPath("//button[contains(text(), 'Register')]"));

            //Click on the log in button
            registerButton.Click();

            //Wait for the system to load the page after logging in
            System.Threading.Thread.Sleep(2000);
        }

        private void loginPatient()
        {
            //Log in process
            //Go to the patient log in form
            driver.Navigate().GoToUrl($"{baseUrl}/Login/Patient/Form");

            //Find the email text box
            var emailInput = driver.FindElement(By.Id("Email"));

            //Find the password text box
            var passwordInput = driver.FindElement(By.Id("Password"));

            //Enter the test patient email
            emailInput.SendKeys("serbe25@gmail.com");

            //Enter the test patient password
            passwordInput.SendKeys("NewPassword25!");

            //Find the log in button
            var loginButton = driver.FindElement(By.XPath("//button[contains(text(), 'Log In')]"));

            //Click on the log in button
            loginButton.Click();

            //Wait for the system to load the page after logging in
            System.Threading.Thread.Sleep(2000);
        }


        [Test]
        public void BookAppointmentProcess()
        {
            //Arrange 

            loginPatient();

            bool errorText = driver.FindElements(By.ClassName("validation-summary-errors")).Count > 0;

            if (errorText)
            {
                registerPatient();
            }
            
            System.Threading.Thread.Sleep(2000);
            //Appointments Page
            //Find the Get An Appointment button
            var getAppointmentButton = driver.FindElement(By.LinkText("Get An Appointment"));

            //Click on the get appointment button
            getAppointmentButton.Click();

            //Doctors Page
            // Find the first row of the doctors table
            var firstDoctorRow = driver.FindElement(By.CssSelector("table tbody tr:first-child"));

            // Find the Book Appointment button for that first doctor
            var bookButton = firstDoctorRow.FindElement(By.CssSelector("a.btn-success")); // Or "a.btn"

            //Click on the Book Appointment button
            bookButton.Click();

            //Wait for the system to load the page
            System.Threading.Thread.Sleep(2000);


            //Act
            //Book Appointments Page
            //Find the first available time slot
            var timeSlotSelect = driver.FindElement(By.Name("ActiveAppointment.AppointmentDate"));
            
            //Create SelectElement
            var selectElement = new SelectElement(timeSlotSelect);

            //Variable to save the selected Time Slot
            string selectedTimeSlot;

            if (selectElement.Options.Count > 1) //If there are more than 1 time slots then continue
            {
                selectElement.SelectByIndex(0); //Select the first available timeslot
                selectedTimeSlot = selectElement.SelectedOption.Text.Trim(); //Trim the text and save it to the selectedTimeSlot variable
            }
            else
            {
                Assert.Fail("No available time slots to book."); //No timeslots left
                return; //fail the test
            }

            // Find the Notes section
            var appointmentNotesInput = driver.FindElement(By.Id("ActiveAppointment_Notes"));

            //Fill in the notes section
            appointmentNotesInput.SendKeys("Headaches for 4 months");

            //Find the save button
            var saveButton = driver.FindElement(By.CssSelector("button[type='submit']"));
            
            //Click the save button
            saveButton.Click();

            //Wait for system to load the page
            System.Threading.Thread.Sleep(2000);

            //Appointments page
            //Find the last element in the table on the appointments page
            var lastAppointmentRow = driver.FindElement(By.CssSelector("table tbody tr:last-child"));

            // Get the time of the latest created appointment
            var dateTimeCellText = lastAppointmentRow.FindElements(By.TagName("td"))[0].Text.Trim(); // Assumes Date & Time is in the 1st column

            //Assert
            //Check that the latest appointment time matches the newly created appointment time
            Assert.That(dateTimeCellText, Is.EqualTo(selectedTimeSlot), "Latest appointment does not match the selected time slot.");
        }
        
        [TearDown]
        public void TearDown()
        {
            driver.Quit();
            driver.Dispose();
        }

    }
}
