using System.Collections.Immutable;

namespace ConcentrationDominos.Models
{
    public static class GameInstructionsModel
    {
        public static ImmutableArray<string> InstructionLines
            = ImmutableArray.Create(
                "Concentration Dominos is a game of memory.",
                "The game is played by flipping over dominos in pairs, in an attempt to match pairs whose values add up to the maximum value in the set.",
                "That is, for a Double-Six domino set, make pairs that add up to 12. For a Double-Nine set, make pairs that add up to 18.",
                "The game is complete when there are no more pairs available.");
    }
}
