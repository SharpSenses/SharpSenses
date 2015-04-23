using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;
using SharpSenses.Client;

namespace SharpSenses.Playground.Universal.Converters {
    public class TopConverter : IValueConverter {

        private int _resolutionHeight;

        public TopConverter() {
            var camera = Camera.Create();
            _resolutionHeight = camera.ResolutionHeight;
        }

        public object Convert(object value, Type targetType, object parameter, string language) {
            int position = System.Convert.ToInt32(value);
            int actualHeight = System.Convert.ToInt32(parameter);
            return position / _resolutionHeight * actualHeight;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }
}
