using System.ComponentModel;
using System.Windows.Input;

namespace OverlayKey
{
    public class KeyViewModel : INotifyPropertyChanged
    {
        public string Label { get; set; }

        public Key Key { get; set; }

        public int GridColumn { get; set; }

        public int ColSpan { get; set; } = 1;

        private bool _isPressed;
        public bool IsPressed
        {
            get => _isPressed;
            set { _isPressed = value; OnPropertyChanged(nameof(IsPressed)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
