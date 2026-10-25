namespace LightsOutGame.Wpf.ViewModels
{
    public class CellViewModel : ViewModelBase
    {
        public bool IsOn { get; set; }

        public int Row { get; set; }

        public int Column { get; set; }
    }
}
