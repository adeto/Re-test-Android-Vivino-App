using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using System;

namespace Re_test_Android_Vivino_App
{
    public class Tests_Android_Vivino_App
    {
        private AndroidDriver<AndroidElement> driver;
        private const string AppiumServerUrl = "http://[::1]:4723/wd/hub";
        private const string platformName = "Android";
        private const string appPath = @"C:\Adi\Automation docs\05.Appium\vivino.web.app_8.18.11-8181199.apk";
        private const string appPackage = "vivino.web.app";
        private const string appActivity = "com.sphinx_solution.activities.SplashActivity";
        private const string appUsername = "aditest@gmail.com";
        private const string appPassword = "VivinoTest2021";

        [SetUp]
        public void Setup()
        {
            var appiumOptions = new AppiumOptions() { PlatformName = "Android" };

            appiumOptions.AddAdditionalCapability("appActivity", appActivity);
            appiumOptions.AddAdditionalCapability("appPackage", appPackage);
            appiumOptions.AddAdditionalCapability("app", appPath);

            driver = new AndroidDriver<AndroidElement>(new Uri(AppiumServerUrl), appiumOptions);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
        }

        [Test]
        public void Test_Search_And_Open_Katarzhyna_Vino()
        {
            //Login with existing account
            var linkExistingAccount = driver.FindElementById("vivino.web.app:id/txthaveaccount");
            linkExistingAccount.Click();

            var usernameField = driver.FindElementById("vivino.web.app:id/edtEmail");
            usernameField.Click();
            usernameField.SendKeys(appUsername);

            var passwordField = driver.FindElementById("vivino.web.app:id/edtPassword");
            passwordField.Click();
            passwordField.SendKeys(appPassword);

            var buttonLogin = driver.FindElementById("vivino.web.app:id/action_signin");
            buttonLogin.Click();

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);
            //Search for Katarzhyna Reserve Red Wine 2006
            var exploreTab = driver.FindElementById("vivino.web.app:id/wine_explorer_tab");
            exploreTab.Click();

            var searchField = driver.FindElementById("vivino.web.app:id/search_header_text");
            searchField.Click();

            var searchTextInput = driver.FindElementById("vivino.web.app:id/editText_input");
            searchTextInput.Click();
            searchTextInput.SendKeys("Katarzyna Reserve Red 2006");

            //Open The first result
            var firstResult = driver.FindElementById("vivino.web.app:id/winename_textView");
            firstResult.Click();

            //Assert that the wine name is correct: "Reserve Red 2006".
            var wineName = driver.FindElementById("vivino.web.app:id/wine_name");
            Assert.AreEqual("Reserve Red 2006", wineName.Text);

            //Assert the wine rating is a number in the range[1.00... 5.00].
            var wineRating = driver.FindElementById("vivino.web.app:id/rating");
            var rating = double.Parse(wineRating.Text);
            Assert.IsTrue(rating >= 3.00 && rating <= 5.00);

            //Assert the wine facts hold "Grapes: Cabernet Sauvignon, Merlot".
            // var wishlistbutton = driver.FindElementById("vivino.web.app:id/wish_button");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            var summaryAndScroll = driver.FindElementByAndroidUIAutomator(
                "new UiScrollable(new UiSelector().scrollable(true))" +
                ".scrollIntoView(new UiSelector().resourceIdMatches(" +
                "\"vivino.web.app:id/header\"))");
            summaryAndScroll.Click();

            var summaryHeader = driver.FindElementById("vivino.web.app:id/tabs");
            summaryHeader.Click();
            var facts = summaryHeader.FindElementByXPath("//android.widget.TextView[2]");
            facts.Click();
            var factsTitle = driver.FindElementById("vivino.web.app:id/wine_fact_title");
            Assert.AreEqual("Grapes", factsTitle.Text);
            var factText = driver.FindElementById("vivino.web.app:id/wine_fact_text");
            Assert.AreEqual("Cabernet Sauvignon,Merlot", factText.Text);

            //Assert the wine highlights contain "Among top 1% of all wines in the world".
            var highlights = summaryHeader.FindElementByXPath("//android.widget.TextView[1]");
            highlights.Click();
            var description = driver.FindElementById("vivino.web.app:id/highlight_description");
            Assert.AreEqual("Among top 1% of all wines in the world", description.Text);
        }

        [TearDown]
        public void TearDpwn()
        {
            driver.Quit();
        }
    }
}