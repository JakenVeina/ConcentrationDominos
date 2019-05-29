using System.Collections.Generic;
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
                .ToArray();
        }

        public ushort Height { get; }

        public IReadOnlyList<GameBoardTileModel> Tiles { get; }

        public ushort Width { get; }
    }
}
