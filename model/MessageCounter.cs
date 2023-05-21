namespace TelegrammService.model
{
    public class MessageCounter
    {

        public int counter { get; set; }
        public int limit { get; set; }

        public MessageCounter()
        {
            resetCounter();
            resetLimit();
        }

        public void resetCounter()
        {
            counter = 0;
        }
        public void resetLimit()
        {
            limit = 0;
        }

        public void count()
        {
            counter++;
        }

        public bool checkLimitExceeded()
        {
            if (limit == 0)
            {
                return false;
            }
            return counter > limit;
        }
    }
}
