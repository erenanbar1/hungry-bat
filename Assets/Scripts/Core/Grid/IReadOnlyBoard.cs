using System.Collections.Generic;
using HungryBat.Core.Items;

namespace HungryBat.Core.Grid
{
    /// <summary>
    /// Read-only view of the board. Rules, effects and scanners take this so they
    /// physically cannot mutate the board while inspecting it -- only the resolvers
    /// that own a turn get the writable <see cref="Board"/>.
    /// </summary>
    public interface IReadOnlyBoard
    {
        int NoColumns { get; }
        int NoRows { get; }

        bool Contains(GridPosition position);

        /// <summary>True when the position is on the board and is not a hole.</summary>
        bool IsPlayable(GridPosition position);

        /// <summary>The item at the position, or null when empty, a hole, or off the board.</summary>
        Item GetItem(GridPosition position);

        /// <summary>Every playable position, column-major from the bottom-left.</summary>
        IEnumerable<GridPosition> PlayablePositions();
    }
}
