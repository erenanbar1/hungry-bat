using HungryBat.Core.Items;

namespace HungryBat.Core.Grid
{
    /// <summary>
    /// A slot on the board. Kept as a class rather than a bare array entry so that
    /// per-cell state (holes now; ice, crates or generators later) has somewhere to live.
    /// </summary>
    public sealed class Cell
    {
        public GridPosition Position { get; }

        /// <summary>False for a hole in the board: nothing rests there and gravity skips it.</summary>
        public bool IsPlayable { get; }

        public Item Item { get; internal set; }
        //IsEmpty propert is derived from Item's value.
        public bool IsEmpty => Item == null;

        public Cell(GridPosition position, bool isPlayable)
        {
            Position = position;
            IsPlayable = isPlayable;
        }

        public override string ToString() => $"{Position} {(IsPlayable ? Item?.ToString() ?? "empty" : "hole")}";
    }
}
