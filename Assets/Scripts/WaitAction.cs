namespace Assets.Scripts
{
    public class WaitAction : ISpawnerAction
    {
        public WaitAction(double time)
        {
            Time = time;
        }

        public double Time { get; private set; }

        public override string ToString()
        {
            return string.Format("wait {0}", Time);
        }
    }
}