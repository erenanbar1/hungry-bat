using System;

namespace HungryBat.Core.Grid
{
    /// <summary>
    /// Immutable board coordinate. Column grows to the right, row grows upward,
    /// so row 0 is the floor that gravity pulls items towards.
    /// </summary>
    public readonly struct GridPosition : IEquatable<GridPosition>
    {
        public int Column { get; }
        public int Row { get; }

        public GridPosition(int column, int row)
        {
            Column = column;
            Row = row;
        }

        //returns the GridPosition that is offset from direction. For example, if the current position is (2,3) and the direction is (1,0), the returned position will be (3,3).
        public GridPosition Offset(GridDirection direction)
        {
            return new GridPosition(Column + direction.ColumnStep, Row + direction.RowStep);
        }
        //returns the GridPosition that is offset from the current position by the specified number of columns and rows. For example, if the current position is (2,3) and the offset is (1,0), the returned position will be (3,3).
        public GridPosition Offset(int columns, int rows)
        {
            return new GridPosition(Column + columns, Row + rows);
        }
        //adjacency is defined as being one step away in either the column or row direction, but not both. For example, (2,3) is adjacent to (2,4) and (3,3), but not to (3,4) or (1,3).
        public bool IsAdjacentTo(GridPosition other)
        {
            return Math.Abs(Column - other.Column) + Math.Abs(Row - other.Row) == 1;
        }

        public bool Equals(GridPosition other) => Column == other.Column && Row == other.Row;

        //if obj is of type GridPosition, cast is to GridPosition under the name other and delegate to the Equals(GridPosition other) method.
        public override bool Equals(object obj) => obj is GridPosition other && Equals(other);
        //397 is a random prime. Collisions may occur but Equals(GridPosition other) will be called to resolve them.
        //Same (row,col) will guarantee same hash, but not vice versa.
        public override int GetHashCode() => unchecked((Column * 397) ^ Row);   

        public override string ToString() => $"({Column},{Row})";

        public static bool operator ==(GridPosition left, GridPosition right) => left.Equals(right);

        public static bool operator !=(GridPosition left, GridPosition right) => !left.Equals(right);
    }
}
