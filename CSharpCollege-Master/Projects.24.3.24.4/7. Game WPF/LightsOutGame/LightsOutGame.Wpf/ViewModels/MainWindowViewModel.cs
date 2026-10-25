using LightsOutGame.Business.Models;
using LightsOutGame.Business.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightsOutGame.Wpf.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private GameService _service = new GameService();

        private GameModel _model = new GameModel();

        public ObservableCollection<CellViewModel> Cells { get; } = new ObservableCollection<CellViewModel>();

        public int MoveCount => _model.MoveCount;

        public MainWindowViewModel()
        {
            Initialize();
        }

        public void Initialize()
        {
            _service.Shuffle(_model);
            LoadViewModel(_model);
        }

        public void MakeMove(CellViewModel cell, Action gameFinishedAction)
        {
            if (_service.IsGameOver(_model))
            {
                return;
            }

            _service.MakeMove(_model, cell.Row, cell.Column);
            LoadViewModel(_model);

            if (_service.IsGameOver(_model))
            {
                gameFinishedAction?.Invoke();
            }
        }

        private void LoadViewModel(GameModel model)
        {
            Cells.Clear();
            for (int row = 0; row < GameModel.RowCount; row++)
            {
                for (int column = 0; column < GameModel.ColumnCount; column++)
                {
                    Cells.Add(new CellViewModel
                    {
                        Row = row,
                        Column = column,
                        IsOn = model[row, column],
                    });
                }
            }

            OnPropertyChanged(nameof(MoveCount));
        }
    }
}
