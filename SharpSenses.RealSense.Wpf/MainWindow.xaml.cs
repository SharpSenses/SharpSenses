using SharpSenses.RealSense.Capabilities;
using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.IO;

namespace SharpSenses.RealSense.Wpf {
    public partial class MainWindow : Window {

        private bool _useSegmentation;

        public MainWindow() {
            InitializeComponent();

            var cam = Camera.Create(Capability.ImageStreamTracking, Capability.SegmentationStreamTracking);
            cam.ImageStream.NewImageAvailable += (s, e) => {
                if (_useSegmentation) return;
                UpdateImage(e.BitmapImage);
            };
            cam.SegmentationStream.NewImageAvailable += (s, e) => {
                if (!_useSegmentation) return;
                UpdateImage(e.BitmapImage);
            };
            cam.Start();
        }

        private void UpdateImage(byte[] imageToUpdate) {
            Dispatcher.BeginInvoke(new Action(() => {
                Video.Source = ToImage(imageToUpdate);
            }));
            
        }

        public BitmapImage ToImage(byte[] array) {
            using (var ms = new MemoryStream(array)) {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            if (_useSegmentation) {
                ButtonSegmentation.Content = "Enable Segmentation";
            }
            else {
                ButtonSegmentation.Content = "Disable Segmentation";
            }
            _useSegmentation = !_useSegmentation;
        }
    }
}
