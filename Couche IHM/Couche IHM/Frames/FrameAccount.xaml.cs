using Couche_IHM.VueModeles;
using System.Windows.Controls;

namespace Couche_IHM.Frames
{
    public partial class FrameAccount : Page
    {

        public FrameAccount()
        {
            InitializeComponent();
            DataContext = MainWindowViewModel.Instance;
        }
    }
}
