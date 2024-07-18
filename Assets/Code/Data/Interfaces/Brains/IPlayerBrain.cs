namespace Tulip.Data
{
    public interface IPlayerBrain : ICharacterBrain, IJumperBrain, IDasherBrain
    {
        public float ZoomDelta { get; }
        public bool WantsToToggleSmartCursor { get; }
        public int HotbarSelectionDelta { get; }
        public int? HotbarSelectionIndex { get; }
    }
}