using System;
using System.Reactive;

namespace ConcentrationDominos.Models
{
    public class GameStateModel
    {
        public GameStateModel()
        {
            DominoSet = new ObservableValue<DominoSetModel>(
                new DominoSetModel(DominoSetType.Empty));
            GameBoard = new ObservableValue<GameBoardModel>(
                new GameBoardModel(0, 0));
            Runtime = new ObservableValue<TimeSpan>(TimeSpan.Zero);
            Settings = new ObservableValue<GameSettingsModel>(
                new GameSettingsModel(DominoSetType.Empty, TimeSpan.Zero));
            State = new ObservableValue<GameState>(GameState.Idle);
        }

        public ObservableValue<DominoSetModel> DominoSet { get; }

        public ObservableValue<GameBoardModel> GameBoard { get; }

        public ObservableValue<TimeSpan> Runtime { get; }

        public ObservableValue<GameSettingsModel> Settings { get; }

        public ObservableValue<GameState> State { get; }

        public TimeSpan UpdateInterval { get; set; }
    }
}
