using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightsOutGame.Business.Models
{
    public class GameModel
    {
        public const int RowCount = 5;

        public const int ColumnCount = 5;

        /// <summary>
        /// true - светильник горит, false - погашен
        /// </summary>
        private bool[,] _cells = new bool[RowCount, ColumnCount];

        public bool this[int row, int column]
        {
            get => _cells[row, column];
            internal set => _cells[row, column] = value;
        }

        /// <summary>
        /// Количество ходов с начала игры
        /// </summary>
        public int MoveCount { get; internal set; }
    }
}
