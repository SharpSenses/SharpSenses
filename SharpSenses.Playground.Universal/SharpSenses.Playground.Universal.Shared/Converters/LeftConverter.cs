using System;
using Windows.UI.Xaml.Data;
using SharpSenses.Client;

namespace SharpSenses.Playground.Universal.Converters {
    public class LeftConverter : IValueConverter {

        private int _resolutionWidth;

        public LeftConverter() {
            var camera = Camera.Create();
            _resolutionWidth = camera.ResolutionWidth;
        }

        public object Convert(object value, Type targetType, object parameter, string language) {
            int position = System.Convert.ToInt32(value);
            int actualWidth = System.Convert.ToInt32(parameter);
            return position / _resolutionWidth * actualWidth;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}