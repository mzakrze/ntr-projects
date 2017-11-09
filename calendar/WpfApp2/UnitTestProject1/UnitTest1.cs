using System;
using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Rhino.Mocks.Impl;
using WpfApp2.View;
using WpfApp2.ViewModel;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        public MainWindowViewModel mainWindowViewModel = new MainWindowViewModel();
        public MainWindow mainWindow = MockRepository.GenerateStub<MainWindow>();

        //public EventViewModel eventViewModel = new EventViewModel();

        [TestMethod]
        public void moveCalenderWeekAheadWithNoEvents()
        {

        }

        [TestMethod]
        public void moveCalenderWeekAheadWithEvents()
        {

        }

        [TestMethod]
        public void moveCalenderWeekBehindWithNoEvents()
        {

        }

        [TestMethod]
        public void moveCalenderWeekBehindWithEvents()
        {

        }

        [TestMethod]
        public void addSomeEvents()
        {
            EventRaiser e;
            mainWindowViewModel.AddEventCommand.Execute(new DateTime(2017, 11, 2));
            EventView eventView = MockRepository.GenerateStub<EventView>();
            eventView.RaiseEvent(new System.Windows.RoutedEventArgs());

            //IUIDataProvider p;

            //this.loadViewModelWithInitalContext(mainWindowViewModel);
            //DateTime date = new DateTime(2017, 11, 2);
            //mainWindowViewModel.V
            //mainWindowViewModel.AddEventCommand.Execute(date);

        }

        [TestMethod]
        public void deleteSomeEvents()
        {

        }

        [TestMethod]
        public void editSomeEvent()
        {

        }

        [TestMethod]
        public void whenUserFillsIncorrectData_ThenUserCantSave()
        {
            EventViewModel eventViewModel = new EventViewModel();
            eventViewModel.Date = "2017/12/13";
            eventViewModel.BeginTime = "12:12";
            eventViewModel.EndTime = "13:13";
            eventViewModel.Name = "name";
            Assert.IsTrue(eventViewModel.SaveCommand.CanExecute(null));

            eventViewModel.Name = "";

            Assert.IsFalse(eventViewModel.SaveCommand.CanExecute(null));
        }

        [TestMethod]
        public void whenUserCancelsEditing_NothingHappens()
        {

        }
    }
}
