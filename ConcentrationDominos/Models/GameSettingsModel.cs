using System;

namespace ConcentrationDominos.Models
{
    public class GameSettingsModel
    {
        public GameSettingsModel(DominoSetType dominoSetType, TimeSpan memoryInterval)
        {
            DominoSetType = dominoSetType;
            MemoryInterval = memoryInterval;
        }

        public DominoSetType DominoSetType { get; }

        public TimeSpan MemoryInterval { get; }
    }
}
