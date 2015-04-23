using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace SharpSenses.Playground.Universal.ViewModels {
    public class ObservableObject : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        protected PropertyChangedEventHandler PropertyChangedHandler {
            get {
                return PropertyChanged;
            }
        }

        protected virtual void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression) {
            if (propertyExpression == null) {
                return;
            }
            var body = propertyExpression.Body as MemberExpression;
            RaisePropertyChanged(body.Member.Name);
        }

        protected virtual void RaisePropertyChanged(string propertyName) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}