using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Tools;
using FlaUI.UIA3;

namespace UiTestsDemo
{
    public class DemoTests
    {
        // you need to change this to actual location of the WpfTotalnik.exe
        private readonly string path = "C:\\Users\\donnie\\source\\repos\\totalnik-master\\bin\\Debug\\WpfTotalnik.exe";
        private Application _app;
        [SetUp]
        public void LauhchApp()
        { 
            _app = Application.Launch(path);
        }
        [TearDown]
        public void Cleanup()
        {
            _app.Close(); 
        }

        [Test]
        public void LaunchAndCloseApp()
        {
            using var automation = new UIA3Automation();
            var window = _app.GetMainWindow(automation);
            Assert.Multiple(() =>
            {
                Assert.That(window.ActualHeight, Is.EqualTo(660), "Actual Height value");
                Assert.That(window.ActualWidth, Is.EqualTo(450), "Actual Width value");
            });
            var popup = window.FindFirstDescendant(cf => cf.ByAutomationId("PART_Toggle")).AsButton();
            popup.Click();
            //this pause and the others below -- for convinience only(all the actions are too fast to see them)
            Thread.Sleep(1000);
            var popupExitBtn = window.FindFirstDescendant(cf => cf.ByAutomationId("PopupExitBtnID")).AsButton();
            Assert.That(popupExitBtn, Is.Not.Null, "popup menu button exists");
            popupExitBtn.Click();
        }

        [Test]
        public void LaunchLoginSuccessExit()
        {
            //starting app and login to it
            using var automation = new UIA3Automation();
            var window = _app.GetMainWindow(automation);
            var userName = window.FindFirstDescendant(cf => cf.ByAutomationId("UsernameID")).AsTextBox();
            userName.Text = "Vitya";
            Thread.Sleep(1000);
            var password = window.FindFirstDescendant(cf => cf.ByAutomationId("PasswordID")).AsTextBox();
            password.Text = "SoftTeco";
            Thread.Sleep(1000);
            var loginBtn = window.FindFirstDescendant(cf => cf.ByAutomationId("LoginBtnID")).AsButton();
            Assert.Multiple(() =>
            {
                Assert.That(loginBtn.IsAvailable, Is.True, "Login button is available");
                Assert.That(loginBtn.IsEnabled, "Login button is enabled");
            });
            loginBtn.Click();

            //cheking menu for nested elements
            window = _app.GetMainWindow(automation);
            var menu = window.FindFirstDescendant(cf => cf.ByAutomationId("MenuID")).AsMenuItem();
            menu.Click();
            Assert.That(menu, Is.Not.Null);
            var items = menu.Items;
            Assert.That(items, Has.Length.EqualTo(2), "Menu has 2 submenu items");
            Assert.Multiple(() =>
            {
                Assert.That(items[0].Properties.Name, Is.EqualTo("Compare Folders"), "First submenu item is Compare Folders");
                Assert.That(items[1].Properties.Name, Is.EqualTo("Exit"), "Second submenu item is Exit");
            });
            Thread.Sleep(1000);
            var exitBtn = window.FindFirstDescendant(cf => cf.ByAutomationId("ExitBtnID")).AsMenuItem();
            Assert.Multiple(() =>
            {
                Assert.That(exitBtn.IsAvailable, Is.True);
                Assert.That(exitBtn.IsEnabled);
            });
            exitBtn.Click();
        }

        [Test]
        public void LaunchLoginFailExit()
        {
            using var automation = new UIA3Automation();
            var window = _app.GetMainWindow(automation);
            var userName = window.FindFirstDescendant(cf => cf.ByAutomationId("UsernameID")).AsTextBox();
            userName.Text = "Vitya";
            Thread.Sleep(1000);
            var password = window.FindFirstDescendant(cf => cf.ByAutomationId("PasswordID")).AsTextBox();
            password.Text = "SoftTec";
            Thread.Sleep(1000);
            var loginBtn = window.FindFirstDescendant(cf => cf.ByAutomationId("LoginBtnID")).AsButton();
            loginBtn.Click();
            Thread.Sleep(1000);
            var closeBtn = window.FindFirstDescendant(cf => cf.ByAutomationId("Close")).AsButton();
            closeBtn.Click();
            Thread.Sleep(1000);
            var popup = window.FindFirstDescendant(cf => cf.ByAutomationId("PART_Toggle")).AsButton();
            popup.Click();
            Thread.Sleep(1000);
            var popupExitBtn = window.FindFirstDescendant(cf => cf.ByAutomationId("PopupExitBtnID")).AsButton();
            Assert.Multiple(() =>
            {
                Assert.That(popupExitBtn.IsAvailable, Is.True);
                Assert.That(popupExitBtn.IsEnabled);
            });
            popupExitBtn.Click();
        }

        [Test]
        public void IsAvailableTest()
        {
            using var automation = new UIA3Automation();
            var window = _app.GetMainWindow(automation);
            Assert.That(window.IsAvailable, Is.True, "Main widnow is opened and available");
            window.Close();
            Retry.WhileTrue(() => window.IsAvailable, TimeSpan.FromSeconds(1));
            Assert.That(window.IsAvailable, Is.False, "Main window is closed an unavailable");
        }

        [Test]
        public void SwitchToDarkMode()
        {
            using var automation = new UIA3Automation();
            var window = _app.GetMainWindow(automation);
            var popup = window.FindFirstDescendant(cf => cf.ByAutomationId("PART_Toggle")).AsButton();
            popup.Click();
            Thread.Sleep(1000);
            var darkModeToggle = window.FindFirstDescendant(cf => cf.ByAutomationId("DarkModeToggleID")).AsToggleButton();
            Assert.Multiple(() =>
            {
                Assert.That(darkModeToggle.IsAvailable, Is.True, "Theme style toggle is available");
                Assert.That(darkModeToggle.IsToggled, Is.False, "Theme style toggle is not toggled");
            });
            Thread.Sleep(1000);
            darkModeToggle.Toggle();
            Assert.That(darkModeToggle.IsToggled, Is.True, "Button is toggled");
            darkModeToggle.Click();
            Thread.Sleep(1000);
            popup.Click();
            var popupExitBtn = window.FindFirstDescendant(cf => cf.ByAutomationId("PopupExitBtnID")).AsButton();
            Assert.Multiple(() =>
            {
                Assert.That(popupExitBtn.IsAvailable, Is.True, "Exit button is available");
                Assert.That(popupExitBtn.IsEnabled, "Exit button is enabled");
            });
            popupExitBtn.Click();
        }
    }
}