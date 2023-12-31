using Couche_IHM.VueModeles;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Controls;

namespace Couche_IHM.Frames
{
    /// <summary>
    /// Logique d'interaction pour FrameLogs.xaml
    /// </summary>
    public partial class FrameLog : Page
    {
        public FrameLog()
        {          
            InitializeComponent();
            DataContext = MainWindowViewModel.Instance;
            MainWindowViewModel.Instance.LogsViewModel.Logs.CollectionChanged += this.Logs_CollectionChanged;
        }

        private void Logs_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            var collection = (ObservableCollection<LogViewModel>)sender!;

            if (collection.Count == 0)
            {
                this.scrollViewer.ScrollToTop();
            }
        }

        private void ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.ExtentHeight < e.ViewportHeight + e.VerticalOffset + 800)
            {
                MainWindowViewModel.Instance.LogsViewModel.LoadNextPage();
            }
        }
    }
}
