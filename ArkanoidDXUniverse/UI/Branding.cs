using System.ComponentModel;
using Windows.UI.Xaml.Media;
using Microsoft.Xna.Framework;

namespace ArkanoidDXUniverse.UI
{
    // Create a class that implements INotifyPropertyChanged.
    public class Branding : INotifyPropertyChanged
    {
        private static Branding _instance;
        private static Color _backgroundColor = Color.DarkBlue;
        private SolidColorBrush _backgroundBrush;
        private SolidColorBrush _boarderBrush;

        private SolidColorBrush _foregroundBrush;

        public Branding()
        {
            _foregroundBrush =
                new SolidColorBrush(Windows.UI.Color.FromArgb(ForegroundColor.A, ForegroundColor.R, ForegroundColor.G,
                    ForegroundColor.B));
            _backgroundBrush =
                new SolidColorBrush(Windows.UI.Color.FromArgb(BackgroundColor.A, BackgroundColor.R, BackgroundColor.G,
                    BackgroundColor.B));
            _boarderBrush =
                new SolidColorBrush(Windows.UI.Color.FromArgb(BoarderColor.A, BoarderColor.R, BoarderColor.G,
                    BoarderColor.B));
        }

        public static Branding Current => _instance ?? (_instance = new Branding());

        public static Color BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                _backgroundColor = value;
                // Call NotifyPropertyChanged when the source property 
                // is updated.
                Current.ForegroundBrush =
                    new SolidColorBrush(Windows.UI.Color.FromArgb(ForegroundColor.A, ForegroundColor.R,
                        ForegroundColor.G, ForegroundColor.B));
                Current.BackgroundBrush =
                    new SolidColorBrush(Windows.UI.Color.FromArgb(BackgroundColor.A, BackgroundColor.R,
                        BackgroundColor.G, BackgroundColor.B));
                Current.BoarderBrush =
                    new SolidColorBrush(Windows.UI.Color.FromArgb(BoarderColor.A, BoarderColor.R, BoarderColor.G,
                        BoarderColor.B));
            }
        }

        public static Color ForegroundColor => Color.Lerp(_backgroundColor, Color.White, .2f);
        public static Color BoarderColor => Color.Lerp(ForegroundColor, Color.Black, .2f);

        // Create the property that will be the source of the binding.
        public SolidColorBrush ForegroundBrush
        {
            get { return _foregroundBrush; }
            set
            {
                _foregroundBrush = value;
                // Call NotifyPropertyChanged when the source property 
                // is updated.
                NotifyPropertyChanged("ForgroundBrush");
            }
        }

        public SolidColorBrush BackgroundBrush
        {
            get { return _backgroundBrush; }
            set
            {
                _backgroundBrush = value;
                // Call NotifyPropertyChanged when the source property 
                // is updated.
                NotifyPropertyChanged("BackgroundBrush");
            }
        }

        public SolidColorBrush BoarderBrush
        {
            get { return _boarderBrush; }
            set
            {
                _boarderBrush = value;
                // Call NotifyPropertyChanged when the source property 
                // is updated.
                NotifyPropertyChanged("BoarderBrush");
            }
        }

        // Declare the PropertyChanged event.
        public event PropertyChangedEventHandler PropertyChanged;

        // NotifyPropertyChanged will raise the PropertyChanged event, 
        // passing the source property that is being updated.
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}