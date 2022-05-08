namespace MagnifyLogistics.Common
{
    public class SystemTime : ISystemTime
    {
        public DateTime GetNow()
            => DateTime.Now;
    }
}
