using System;

namespace ChainReaction
{
    /// <summary>
    /// This is a game jam, I decided not to use event actions and instead go for fast implementation
    /// </summary>
    public static class StaticActionProvider
    {
        //public static Action<int> setExplosivesCount;
        public static Action ExplosivesPlaced;

        public static Action<float> DestructionForce;

        public static Action TriggerExplosion;

        public static Action RecalculateChainReaction;

        public static Action UpdateForceText;
    }
}
