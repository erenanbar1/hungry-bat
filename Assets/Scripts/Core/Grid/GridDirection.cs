using System;

namespace HungryBat.Core.Grid
{
    /// <summary>A unit step across the board. Used by scans, gravity and swap validation.</summary>
    public readonly struct GridDirection : IEquatable<GridDirection>
    {
        public int ColumnStep { get; }
        public int RowStep { get; }

        public GridDirection(int columnStep, int rowStep)
        {
            ColumnStep = columnStep;
            RowStep = rowStep;
        }

        public static GridDirection Right => new GridDirection(1, 0);
        public static GridDirection Left => new GridDirection(-1, 0);
        public static GridDirection Up => new GridDirection(0, 1);
        public static GridDirection Down => new GridDirection(0, -1);

        public GridDirection Opposite => new GridDirection(-ColumnStep, -RowStep);

        public bool IsHorizontal => RowStep == 0;

        public bool Equals(GridDirection other) => ColumnStep == other.ColumnStep && RowStep == other.RowStep;

        public override bool Equals(object obj) => obj is GridDirection other && Equals(other);

        public override int GetHashCode() => unchecked((ColumnStep * 397) ^ RowStep);

        public override string ToString() => $"[{ColumnStep},{RowStep}]";
    }
}
