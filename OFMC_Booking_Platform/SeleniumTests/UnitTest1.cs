using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;


namespace SeleniumTests
{
    public class Tests
    {
        IWebDriver driver;
        private string baseUrl = "http://localhost:5058";

        [SetUpAttribute]
        public void Setup()
        {
            driver = new ChromeDriver();
        }

        [Test]
        public void Navigation_BookAppointmentToDisplaysDoctorList()
        {
            //Arrange
            driver.Navigate().GoToUrl($"{baseUrl}/appointments");  //start test at the appointments page

            System.Threading.Thread.Sleep(2000); //wait for appointments page to load

            string firstDoctorName = "Dr. Emily Carter"; //set the expected first doctors name

            //Act 
            var saveButton = driver.FindElement(By.LinkText("Get An Appointment")); //find the Get An Appointment button
            saveButton.Click(); //click the Get An Appointment Button

            System.Threading.Thread.Sleep(2000); //wait for doctor page to load

            //Assert
            var doctorRows = driver.FindElements(By.CssSelector("table tbody tr")); //find the table on the page
            var firstDoctorRow = doctorRows.First(); //find the first element in the table
            var physicanNameCellText = firstDoctorRow.FindElements(By.TagName("td"))[0].Text.Trim(); //find the name of the first physician in the table

            Assert.That(physicanNameCellText, Is.EqualTo(firstDoctorName), "First doctor name does not match expected first doctor name"); //check that the first name of the physican matches the expected value
        }

        [Test]
        public void BookAppointmentProcess()
        {
            //Arrange
            driver.Navigate().GoToUrl($"{baseUrl}/doctor/book-appointment-form?id=6"); //load the book appointment form for doctor with id 6

            var timeSlotSelect = driver.FindElement(By.Name("ActiveAppointment.AppointmentDate")); //find the AppointmentDate element
            var selectElement = new SelectElement(timeSlotSelect); //create a new SelectElement
            
            string selectedTimeSlot = "No slot selected"; //starting value of expected result time slot
            

            selectElement.SelectByIndex(0); //select the first time slot 
            selectedTimeSlot = selectElement.SelectedOption.Text.Trim();  //set the time slot as a string, this will be the expected value
            
            if (selectedTimeSlot == "No available slots") //check if an actual time slot was selected, if not fail the test
            {
                Assert.Fail("No available time slots to book."); //no time slots available, so test fails

            }

            var appointmentNotes = driver.FindElement(By.Id("ActiveAppointment_Notes")); //find the notes element
            appointmentNotes.SendKeys("Reoccuring Headaches for 4 months"); //fill in the notes element

            //Act 
            var saveButton = driver.FindElement(By.CssSelector("button[type='submit']")); //find the save button
            saveButton.Click(); //click the save button

            //Assert 
            System.Threading.Thread.Sleep(2000); //sleep for 2000 seconds

            driver.Navigate().GoToUrl($"{baseUrl}/appointments"); //navigatet to the appointments url

            System.Threading.Thread.Sleep(2000); //sleep for 2000 seconds

            var appointmentRows = driver.FindElements(By.CssSelector("table tbody tr")); //find the appoitnments table

            var latestAppointmentRow = appointmentRows.Last(); //find the newest element in the table
            var dateTimeCellText = latestAppointmentRow.FindElements(By.TagName("td"))[0].Text.Trim(); // gets the appointment date

            Assert.That(dateTimeCellText, Is.EqualTo(selectedTimeSlot), "Latest appointment does not match the selected time slot."); //checks that the appointment date matches the appoinment we just made
        }

        [TearDownAttribute]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}
