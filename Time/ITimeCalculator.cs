using Microsoft.Xna.Framework;
namespace KirbyNightmareInDreamLand.Time
{
    public interface ITimeCalculator
    {
        public double GetCurrentTimeInMS(GameTime time);
        public double GetCurrentTimeInS(GameTime time);
        public double GetElapsedTimeInMS(GameTime time);
        public double GetElapsedTimeInS(GameTime time);
    }
}
