using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FKAroundTIA.Models
{
    public class FaceplateInterfaceItemInfo : INotifyPropertyChanged
    {
        private string _propertyName;
        public string PropertyName
        {
            get => _propertyName;
            set { _propertyName = value; OnPropertyChanged(); }
        }

        private string _dataType;
        public string DataType
        {
            get => _dataType;
            set { _dataType = value; OnPropertyChanged(); }
        }

        private object _value;
        public object Value
        {
            get => _value;
            set
            {
                if (!Equals(_value, value))
                {
                    _value = value;
                    OnPropertyChanged();
                    Modified = true;
                }
            }
        }

        private string _binding;
        public string Binding
        {
            get => _binding;
            set
            {
                if (_binding != value)
                {
                    _binding = value;
                    OnPropertyChanged();
                    Modified = true;
                }
            }
        }

        private bool _modified;
        public bool Modified
        {
            get => _modified;
            set { _modified = value; OnPropertyChanged(); }
        }

        public FaceplateInterfaceItemInfo() { }

        public FaceplateInterfaceItemInfo(string propertyName, string dataType, object value, string binding)
        {
            _propertyName = propertyName;
            _dataType = dataType;
            _value = value;
            _binding = binding;
            _modified = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
