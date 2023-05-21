using System.Collections.Generic;

namespace TelegrammService.model
{
    public class ClientMessageStatistic
    {
        public Dictionary<long, int> clientMessageCounter { get; set; }
    }
}
