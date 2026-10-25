using LightsOutGame.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightsOutGame.Business.Services
{
    public class GameService
    {
        private readonly Random _random = new Random();

        /// <summary>
        /// Все светильники погашены
        /// </summary>
        public void Initialize(GameModel model)
        {
            for (int row = 0; row < GameModel.RowCount; row++)
            {
                for (int column = 0; column < GameModel.ColumnCount; column++)
                {
                    model[row, column] = false;
                }
            }

            model.MoveCount = 0;
        }

        /// <summary>
        /// Нажатие на светильник переключает его и четырёх соседей (сверху, снизу, слева, справа)
        /// </summary>
        public bool MakeMove(GameModel model, int row, int column)
        {
            if (row < 0 || row >= GameModel.RowCount || column < 0 || column >= GameModel.ColumnCount)
            {
                return false;
            }

            Toggle(model, row, column);
            Toggle(model, row - 1, column);
            Toggle(model, row + 1, column);
            Toggle(model, row, column - 1);
            Toggle(model, row, column + 1);

            model.MoveCount++;
            return true;
        }

        /// <summary>
        /// Новая игра. Поле получается случайными нажатиями на погашенном поле,
        /// поэтому его всегда можно погасить обратно.
        /// </summary>
        public void Shuffle(GameModel model)
        {
            Initialize(model);

            int presses = _random.Next(8, 16);
            for (int i = 0; i < presses; i++)
            {
                MakeMove(model, _random.Next(GameModel.RowCount), _random.Next(GameModel.ColumnCount));
            }

            if (IsGameOver(model))
            {
                MakeMove(model, _random.Next(GameModel.RowCount), _random.Next(GameModel.ColumnCount));
            }

            model.MoveCount = 0;
        }

        /// <summary>
        /// Игра окончена, когда погашены все светильники
        /// </summary>
        public bool IsGameOver(GameModel model)
        {
            for (int row = 0; row < GameModel.RowCount; row++)
            {
                for (int column = 0; column < GameModel.ColumnCount; column++)
                {
                    if (model[row, column])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void Toggle(GameModel model, int row, int column)
        {
            if (row < 0 || row >= GameModel.RowCount || column < 0 || column >= GameModel.ColumnCount)
            {
                return;
            }

            model[row, column] = !model[row, column];
        }
    }
}
