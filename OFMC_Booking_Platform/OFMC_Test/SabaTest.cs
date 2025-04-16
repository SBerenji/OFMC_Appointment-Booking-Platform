using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Buffers.Text;


namespace OFMC_Test
{
    internal class SabaTest
    {
        private IWebDriver driver;

        private string baseUrl = "https://localhost:7276";

        //WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
        }


        // to make sure everything cleans up and gets disposed after the test is done
        [TearDown]
        public void TearDown()
        {
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
            }
        }

        [Test]
        public void CancelPatientAppointment_Admin()
        {

            //*********** Logging into the admin account ***********

            //Navigating to the admin log in form
            driver.Navigate().GoToUrl($"{baseUrl}/Login/Admin/Form");

            // Find the admin email input field and enter the email
            driver.FindElement(By.Id("AdminEmail")).SendKeys("saba@ofmc.com");

            // Find the admin password input field
            driver.FindElement(By.Id("AdminPassword")).SendKeys("Saba@456");


            //Find the log in button and click on it
            driver.FindElement(By.Id("AdminLogIn")).Click();


            //*********** Checking to make sure all doctors are present ***********

            // Find the doctors table list
            var tableRows = driver.FindElements(By.CssSelector("table tbody tr"));

            // extract the content of the first column of the table which contains the doctors name
            List<string> actualDoctorNames = tableRows
            .Select(row => row.FindElements(By.TagName("td"))[0].Text.Trim())
            .ToList();


            // Creating a list of all the doctor names that must be present in the table
            List<string> expectedDoctorNames = new List<string>
            {
                "Dr. Emily Carter",
                "Dr. Shawn Kieze",
                "Dr. Sophia Lee",
                "Dr. James Thompson",
                "Dr. Olivia Martinez",
                "Dr. Ryan Patel"
            };


            // checking if the expected names are present in the actual names extracted from the table
            foreach (var expectedDoctorName in expectedDoctorNames)
            {
                Assert.That(actualDoctorNames, Does.Contain(expectedDoctorName));

            }


            driver.FindElement(By.Id("view-appointments-1")).Click(); ; // DoctorId = 1 / Doctor Carter


            string targetPatient = "Saba Berenji";



            // have a loop that delets all the appointments related to a specific patient

            while (true)
            {
                try
                {
                    var rows = driver.FindElements(By.CssSelector("table tbody tr"));  // find the table of the doctor appointments
                    bool found = false;

                    foreach (var row in rows)
                    {
                        // extract the patient name from the first column of the table
                        var nameCell = row.FindElements(By.TagName("td"))[0];

                        if (nameCell.Text.Trim() == targetPatient)
                        {
                            found = true;

                            // Find the cancel button and click on it
                            row.FindElement(By.Id("cancel")).Click();


                            // Wait for confirmation page to load
                            //wait.Until(driver => driver.FindElement(By.CssSelector("form button[type='submit']")));
                            Thread.Sleep(2000);

                            // Confirm appointment cancellation by clicking on Yes
                            driver.FindElement(By.CssSelector("form button[type = 'submit']")).Click();

                            Thread.Sleep(3000);

                            break;
                        }

                    }

                    if (!found)
                    {
                        Console.WriteLine("No more appointments found for " + targetPatient);
                        break;
                    }
                }

                catch (Exception ex)
                {
                    Console.WriteLine("Patient Not Found", ex);
                    break;
                }


            }

        }
    }
}
