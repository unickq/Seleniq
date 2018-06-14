using System.Diagnostics;

namespace Seleniq.Util
{
    public class PerformanceTool
    {
        private readonly Stopwatch _timer;

        public PerformanceTool()
        {
            _timer = new Stopwatch();
        }

        public void StartMeasure()
        {
            _timer.Reset();
            _timer.Start();
        }

        public string StopMeasure(string value)
        {
            _timer.Stop();
            return $"Load Time of {value}: {_timer.ElapsedMilliseconds}";
        }
    }
}