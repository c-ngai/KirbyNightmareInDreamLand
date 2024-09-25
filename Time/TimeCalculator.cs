using Microsoft.Xna.Framework;

namespace MasterGame
{
    public class TimeCalculator : ITimeCalculator
    {
        public TimeCalculator()
        {
        }
        public double GetCurrentTimeInMS(GameTime time)
        {
            return time.TotalGameTime.TotalMilliseconds;
        }

        public double GetCurrentTimeInS(GameTime time)
        {
            return time.TotalGameTime.TotalSeconds;
        }

        public double GetElapsedTimeInMS(GameTime time)
        {
            return time.ElapsedGameTime.TotalMilliseconds;
        }

        public double GetElapsedTimeInS(GameTime time)
        {
            return time.ElapsedGameTime.TotalSeconds;
        }
    }
}
