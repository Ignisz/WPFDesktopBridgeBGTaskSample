
namespace BackgroundTask
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Windows.ApplicationModel.Background;
    using Windows.Storage;

    public sealed class TestBackgroundTask : IBackgroundTask
    {
        private BackgroundTaskDeferral defferal;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            this.defferal = taskInstance.GetDeferral();
            taskInstance.Canceled += OnCanceled;

            await ExecuteActionAsync();

            this.Finish();
        }

        private async Task ExecuteActionAsync()
        {
            StorageFolder folder = null;
            try
            {
                folder = await KnownFolders.DocumentsLibrary.CreateFolderAsync("SampleFolder", CreationCollisionOption.OpenIfExists);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            if (folder != null)
                await folder.CreateFileAsync("text.bgtxt", CreationCollisionOption.OpenIfExists);
        }

        private void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            this.Finish();
        }

        private void Finish()
        {
            if (this.defferal != null)
            {
                this.defferal.Complete();
                this.defferal = null;
            }
        }
    }
}
