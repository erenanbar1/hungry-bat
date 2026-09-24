using System;
using System.Collections.Generic;
using System.Text;
using HungryBat.Core.Items;

namespace HungryBat.Core.Grid
{
    /// <summary>
    /// The mutable playfield. Deliberately dumb: it stores items and answers questions
    /// about them. Every rule about what *should* happen lives in a resolver or a strategy,
    /// which keeps this class from turning into the usual match-3 god object.
    /// </summary>

    /// Sealed because it is not intended to be inherited from. 
    public sealed class Board : IReadOnlyBoard 
    {
        //2D array of cells, indexed by column and row. The first index is the column, the second is the row.
        private readonly Cell[,] _cells; 

        public int NoColumns { get; }
        public int NoRows { get; }

        /// <param name="isPlayable">
        /// Optional mask for boards that are not a full rectangle. Null means every cell plays.
        /// </param>
        /// Delegate or lambda function is a parameter. 
        public Board(int columns, int rows, Func<GridPosition, bool> isPlayable = null)
        {
            if (columns <= 0) throw new ArgumentOutOfRangeException(nameof(columns));
            if (rows <= 0) throw new ArgumentOutOfRangeException(nameof(rows));

            NoColumns = columns;
            NoRows = rows;
            _cells = new Cell[NoColumns, NoRows];

            for (int column = 0; column < NoColumns; column++)
            {
                for (int row = 0; row < NoRows; row++)
                {
                    var position = new GridPosition(column, row);
                    _cells[column, row] = new Cell(position, isPlayable?.Invoke(position) ?? true);
                }
            }
        }

        public bool Contains(GridPosition position)
        {
            return position.Column >= 0 && position.Column < NoColumns
                && position.Row >= 0 && position.Row < NoRows;
        }

        public bool IsPlayable(GridPosition position)
        {
            return Contains(position) && _cells[position.Column, position.Row].IsPlayable;
        }

        public Cell GetCell(GridPosition position)
        {
            return Contains(position) ? _cells[position.Column, position.Row] : null;
        }

        public Item GetItem(GridPosition position)
        {
            return Contains(position) ? _cells[position.Column, position.Row].Item : null;
        }

        public void PlaceItem(GridPosition position, Item item)
        {
            var cell = GetCell(position);
            if (cell == null) throw new ArgumentOutOfRangeException(nameof(position), $"{position} is off the board.");
            if (!cell.IsPlayable) throw new InvalidOperationException($"{position} is a hole and cannot hold an item.");

            cell.Item = item;
        }

        /// <summary>Empties the cell and returns whatever was there (null when it was already empty).</summary>
        public Item RemoveItem(GridPosition position)
        {
            var cell = GetCell(position);
            if (cell == null) return null;

            var item = cell.Item;
            cell.Item = null;
            return item;
        }

        /// <summary>
        /// Exchanges the contents of two cells. Used both for real player moves and for the
        /// speculative swaps the deadlock scanner performs, which is why it is undo-able by
        /// simply calling it again with the same arguments.
        /// </summary>
        public void SwapItems(GridPosition first, GridPosition second)
        {
            var firstCell = GetCell(first);
            var secondCell = GetCell(second);
            if (firstCell == null || secondCell == null)
                throw new ArgumentOutOfRangeException(nameof(first), "Cannot swap a position that is off the board.");

            (firstCell.Item, secondCell.Item) = (secondCell.Item, firstCell.Item);
        }
        //lazy iterator that yields on request.
        public IEnumerable<GridPosition> PlayablePositions()
        {
            for (int column = 0; column < NoColumns; column++)
            {
                for (int row = 0; row < NoRows; row++)
                {
                    var cell = _cells[column, row];
                    if (cell.IsPlayable) yield return cell.Position;
                }
            }
        }

        /// <summary>Row-major dump with the top row first, for test failures and debug logs.</summary>
        public override string ToString()
        {
            var builder = new StringBuilder();
            for (int row = NoRows - 1; row >= 0; row--)
            {
                for (int column = 0; column < NoColumns; column++)
                {
                    var cell = _cells[column, row];
                    if (!cell.IsPlayable) builder.Append('#');
                    else if (cell.IsEmpty) builder.Append('.');
                    else builder.Append(cell.Item.IsSpecial
                        ? char.ToUpperInvariant(cell.Item.Color.ToString()[0])
                        : char.ToLowerInvariant(cell.Item.Color.ToString()[0]));
                    builder.Append(' ');
                }
                builder.AppendLine();
            }
            return builder.ToString();
        }
    }
}
