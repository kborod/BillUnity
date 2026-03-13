using System;

namespace Kborod.Services.ServerTime
{
    public class TimeService
    {
        public long CurrTimestamp => DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        public long CurrTimestampMs => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        public TimeSpan CurrTimestampTimeSpan => TimeSpan.FromSeconds(CurrTimestamp);

        //DateTime someDate = DateTime.Now;  // или DateTime.UtcNow, или любая другая дата
        //long unixSeconds = ((DateTimeOffset)someDate.ToUniversalTime()).ToUnixTimeSeconds();
    }
}