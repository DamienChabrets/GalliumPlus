
using Couche_IHM.VueModeles;
using System.Windows.Controls;

namespace Couche_IHM.Frames
{
    /// <summary>
    /// Logique d'interaction pour FrameStatistique.xaml
    /// </summary>
    public partial class FrameStatistique : Page
    {
        /// <summary>
        /// Constructeur naturelle 
        /// </summary>
        public FrameStatistique()
        {
            InitializeComponent();
            DataContext = MainWindowViewModel.Instance;
        }
    }
}
