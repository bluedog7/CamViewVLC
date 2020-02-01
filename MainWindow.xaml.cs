using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Ozeki.Media.MediaHandlers.IPCamera;
using Ozeki.Media.MediaHandlers.Video;
using Ozeki.Media.Video.Controls;
using Ozeki.Media.IPCamera;
using Ozeki.Media.MediaHandlers;

namespace CamViewVLC
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private IIPCamera _camera;
        private DrawingImageProvider _drawingImageProvider;
        private MediaConnector _connector;
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
