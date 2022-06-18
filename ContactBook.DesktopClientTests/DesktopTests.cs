using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;

namespace ContactBook.DesktopClientTests
{
    public class DesktopTests
    {
        
        private const string AppiumUrl = "http://127.0.0.1:4723/wd/hub";
        private const string ContactBookUrl = "https://contactbook.nakov.repl.co/api";
        private const string appLocation = @"C:\Work\ContactBook-DesktopClient\ContactBook-DesktopClient.exe";

        private WindowsDriver<WindowsElement> driver;
        private AppiumOptions options;


        [SetUp]
        public void StartApp()
        {
            
            options = new AppiumOptions() { PlatformName = "Windows" };
            options.AddAdditionalCapability("app", appLocation);
            //options.AddAdditionalCapability(MobileCapabilityType.App, appLocation);


            driver = new WindowsDriver<WindowsElement>(new Uri(AppiumUrl), options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [TearDown]  
        public void CloseApp()
        {
            driver.Quit();
        }


        [Test]
        public void Test_SerchContact_VerifyFirstResult()
        {
            //Arrange
            var urlField = driver.FindElementByAccessibilityId("textBoxApiUrl");
            urlField.Clear();
            urlField.SendKeys(ContactBookUrl);

            var buttonConnect = driver.FindElementByAccessibilityId("buttonConnect");
            buttonConnect.Click();

            string windowsName = driver.WindowHandles[0];
            driver.SwitchTo().Window(windowsName);

            var editTextField = driver.FindElementByAccessibilityId("textBoxSearch");
            editTextField.SendKeys("Steve");

            //Act
            var buttonSearch = driver.FindElementByAccessibilityId("buttonSearch");
            buttonSearch.Click();

            //Thread.Sleep(2000);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            var element = wait.Until(d =>
            {
                var searchLabel = driver.FindElementByAccessibilityId("labelResult").Text;
                return searchLabel.StartsWith("Contacts found");
            });
            var searchLabel = driver.FindElementByAccessibilityId("labelResult").Text;
            Assert.That(searchLabel, Is.EqualTo("Contacts found: 1"));

            //Assert
            var firstName = driver.FindElement(By.XPath("//Edit[@Name=\"FirstName Row 0, Not sorted.\"]"));
            var lastName = driver.FindElement(By.XPath("//Edit[@Name=\"LastName Row 0, Not sorted.\"]"));

            Assert.That(firstName.Text, Is.EqualTo("Steve"));
            Assert.That(lastName.Text, Is.EqualTo("Jobs"));

        }
    }
}