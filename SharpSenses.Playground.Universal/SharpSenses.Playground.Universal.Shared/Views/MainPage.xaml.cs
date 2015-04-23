using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SharpSenses.Client;
using SharpSenses.Playground.Universal.ViewModels;

namespace SharpSenses.Playground.Universal {
    public sealed partial class MainPage : Page {

        private MainViewModel _viewModel;
        private ICamera _camera;


        public MainPage() {
            InitializeComponent();
            _camera = Camera.Create(Dispatcher);
            _viewModel = new MainViewModel(_camera);
            DataContext = _viewModel;

            Loaded+= OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs) {
            _camera.Start();
        }
    }
}
