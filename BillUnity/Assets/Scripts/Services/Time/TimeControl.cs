using System;

namespace Kborod.Services.ServerTime
{
    public class TimeControl
    {
        public long CurrTimestamp => DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        //DateTime someDate = DateTime.Now;  // или DateTime.UtcNow, или любая другая дата
        //long unixSeconds = ((DateTimeOffset)someDate.ToUniversalTime()).ToUnixTimeSeconds();
    }
}