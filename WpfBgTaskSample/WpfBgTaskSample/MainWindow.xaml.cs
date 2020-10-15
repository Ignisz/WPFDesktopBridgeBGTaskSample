using System;
using System.Threading.Tasks;
using System.Windows;
using Windows.ApplicationModel.Background;

namespace WpfBgTaskSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void RegisterBackgroundTasks(object sender, RoutedEventArgs e)
        {
            await RegisterBackgroundTasksAsync();
        }

        private async Task RegisterBackgroundTasksAsync()
        {
            await BackgroundExecutionManager.RequestAccessAsync();

            var entryPoint = "BackgroundTask.TestBackgroundTask";
            var condition = new SystemCondition(SystemConditionType.InternetAvailable);

            // check with SYSTEM trigger (when internet available)
            var internetAvailableTrigger = new SystemTrigger(SystemTriggerType.InternetAvailable, false);
            RegisterBackgroundTask(entryPoint, "TestBackgroundTask", internetAvailableTrigger, null);

            // check with Maintenance trigger (when internet available)
            var maintenanceTriggerPeriod = TimeSpan.FromDays(1);
            var timeTrigger = new MaintenanceTrigger((uint)maintenanceTriggerPeriod.TotalMinutes, false);
            RegisterBackgroundTask(entryPoint, "TestBackgroundTask", timeTrigger, condition);
        }

        private void RegisterBackgroundTask(string entryPoint, string taskName, IBackgroundTrigger trigger, IBackgroundCondition condition)
        {
            var builder = new BackgroundTaskBuilder { Name = taskName, TaskEntryPoint = entryPoint };
            builder.SetTrigger(trigger);

            if (condition != null)
            {
                builder.AddCondition(condition);
            }

            try
            {
                builder.Register();
            }
            catch (Exception) { }
        }
    }
}
