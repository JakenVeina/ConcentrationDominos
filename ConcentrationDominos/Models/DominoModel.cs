namespace ConcentrationDominos.Models
{
    public class DominoModel
    {
        public DominoModel(ushort firstSuit, ushort secondSuit)
        {
            FirstSuit = firstSuit;
            SecondSuit = secondSuit;
        }

        public ushort FirstSuit { get; }

        public ushort SecondSuit { get; }
    }
}
