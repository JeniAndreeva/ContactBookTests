using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Linq;



namespace ContactBook.WebDriverTests
{
    public class UI_Tests
    {
        private const string url = "https://contactbook.nakov.repl.co/";
        private WebDriver driver;

        [SetUp]
        public void OpenBrowser()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

        }
        [TearDown]
        public void CloseBrowser()
        {
            driver.Quit();
        }

        [Test]
        public void Test_ListContact_CheckFirstContact()
        {
            //Arrange
            driver.Navigate().GoToUrl(url);
            var contactLink = driver.FindElement(By.CssSelector("body > aside > ul > li:nth-child(2) > a"));

            //Act
            contactLink.Click();

            //Assert
            var firstName = driver.FindElement(By.CssSelector("#contact1 > tbody > tr.fname > td")).Text;
            var lastName = driver.FindElement(By.CssSelector("#contact1 > tbody > tr.lname > td")).Text;

            Assert.That(firstName, Is.EqualTo("Steve"));
            Assert.That(lastName, Is.EqualTo("Jobs"));

        }
        [Test]
        public void Test_SearchContacts_CheckFirstResult()
        {
            //Arrange
            driver.Navigate().GoToUrl(url);
            var searchLink = driver.FindElement(By.CssSelector("body > aside > ul > li:nth-child(4) > a"));

            //Act
            searchLink.Click();
            var searchField = driver.FindElement(By.Id("keyword"));
            searchField.SendKeys("Albert");
            var searchButton = driver.FindElement(By.Id("search"));
            searchButton.Click();

            //Assert
            var firstName = driver.FindElement(By.CssSelector("#contact3 > tbody > tr.fname > td")).Text;
            var lastName = driver.FindElement(By.CssSelector("#contact3 > tbody > tr.lname > td")).Text;

            Assert.That(firstName, Is.EqualTo("Albert"));
            Assert.That(lastName, Is.EqualTo("Einstein"));

        }

        [Test]
        public void Test_SearchContacts_EmptyResult()
        {
            //Arrange
            driver.Navigate().GoToUrl(url);
            var searchLink = driver.FindElement(By.CssSelector("body > aside > ul > li:nth-child(4) > a"));

            //Act
            searchLink.Click();
            var searchField = driver.FindElement(By.Id("keyword"));
            searchField.SendKeys("invalid2635");
            var searchButton = driver.FindElement(By.Id("search"));
            searchButton.Click();

            //Assert
            var resultMessage = driver.FindElement(By.Id("searchResult")).Text;
            Assert.That(resultMessage, Is.EqualTo("No contacts found."));

        }

        [Test]
        public void Test_CreateContacts_InvalidData()
        {
            //Arrange
            driver.Navigate().GoToUrl(url);
            var createLink = driver.FindElement(By.CssSelector("body > aside > ul > li:nth-child(3) > a"));

            //Act
            createLink.Click();
            var firstName = driver.FindElement(By.Id("firstName"));
            firstName.SendKeys("invalid2635");
            var createButton = driver.FindElement(By.Id("create"));
            createButton.Click();

            //Assert
            var errortMessage = driver.FindElement(By.CssSelector("body > main > div")).Text;
            Assert.That(errortMessage, Is.EqualTo("Error: Last name cannot be empty!"));

        }

        [Test]
        public void Test_CreateContacts_ValidData()
        {
            //Arrange
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Create")).Click();

            var firstName = "Jeni" + DateTime.Now.Ticks;
            var lastName = "Milova" + DateTime.Now.Ticks;
            var email = DateTime.Now.Ticks + "jenimilova@bbb.bb";
            var phone = "0888888888";

            //Act
            driver.FindElement(By.Id("firstName")).SendKeys(firstName);
            driver.FindElement(By.Id("lastName")).SendKeys(lastName);
            driver.FindElement(By.Id("email")).SendKeys(email);
            driver.FindElement(By.Id("phone")).SendKeys(phone);

            var createButton = driver.FindElement(By.Id("create"));
            createButton.Click();

            //Assert

            var allContacts = driver.FindElements(By.CssSelector("table.contact-entry"));
            var lastContact = allContacts.Last();

            var firstNameLabel = lastContact.FindElement(By.CssSelector("tr.fname > td")).Text;
            var lastNameLabel = lastContact.FindElement(By.CssSelector("tr.lname > td")).Text;

            Assert.That(firstNameLabel, Is.EqualTo(firstName));
            Assert.That(lastNameLabel, Is.EqualTo(lastName));

        }
    }
}