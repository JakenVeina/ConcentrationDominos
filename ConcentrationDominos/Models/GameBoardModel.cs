using System.Collections.Immutable;
using System.Linq;

namespace ConcentrationDominos.Models
{
    public class GameBoardModel
    {
        public GameBoardModel(ushort width, ushort height)
        {
            Width = width;
            Height = height;

            Tiles = Enumerable.Range(0, height)
                .SelectMany(rowIndex => Enumerable.Range(0, width)
                    .Select(colIndex => new GameBoardTileModel(
                        (ushort)colIndex,
                        (ushort)rowIndex)))
                .ToImmutableArray();
        }

        public ushort Height { get; }

        public ImmutableArray<GameBoardTileModel> Tiles { get; }

        public ushort Width { get; }
    }
}
