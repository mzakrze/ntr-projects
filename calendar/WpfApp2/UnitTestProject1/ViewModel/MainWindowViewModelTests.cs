using Microsoft.VisualStudio.TestTools.UnitTesting;
using WpfApp2.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.ViewModel.Tests
{
    [TestClass()]
    public class MainWindowViewModelTests
    {
        [TestMethod()]
        public void MainWindowViewModelTest()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void whenUserFillsIncorrectData_ThenUserCantSave()
        {
            EventViewModel eventViewModel = new EventViewModel();
            eventViewModel.Date = "2017/12/13";
            eventViewModel.BeginTime = "12:12";
            eventViewModel.EndTime = "13:13";
            eventViewModel.Name = "";
            Assert.IsTrue(eventViewModel.SaveCommand.CanExecute(null));

            eventViewModel.Name = "";

            Assert.IsFalse(eventViewModel.SaveCommand.CanExecute(null));
        }
    }
}