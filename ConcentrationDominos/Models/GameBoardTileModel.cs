using System.Reactive;

namespace ConcentrationDominos.Models
{
    public class GameBoardTileModel
    {
        public GameBoardTileModel(ushort colIndex, ushort rowIndex)
        {
            ColIndex = colIndex;
            Content = new ObservableValue<DominoModel>();
            IsFaceUp = new ObservableValue<bool>();
            IsRotated = new ObservableValue<bool>();
            RowIndex = rowIndex;
        }

        public ushort ColIndex { get; }

        public ObservableValue<DominoModel> Content { get; }

        public ObservableValue<bool> IsFaceUp { get; }

        public ObservableValue<bool> IsRotated { get; }

        public ushort RowIndex { get; }
    }
}
