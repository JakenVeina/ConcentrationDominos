using System.Collections.Generic;
using System.Linq;

namespace ConcentrationDominos.Models
{
    public class DominoSetModel
    {
        public DominoSetModel(DominoSetType type)
        {
            Type = type;

            var suits = Enumerable.Range(0, (int)type + 1)
                .Select(x => (ushort)x);

            Dominos = suits
                .SelectMany(firstSuit => suits
                    .Where(secondSuit => firstSuit <= secondSuit)
                    .Select(secondSuit => new DominoModel(firstSuit, secondSuit)))
                .ToArray();
        }

        public DominoSetType Type { get; }

        public IReadOnlyList<DominoModel> Dominos { get; }
    }
}
