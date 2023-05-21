using TelegrammService.model;

namespace TelegrammService.service
{
    public class MessageCounterService
    {

        public Dictionary<long, MessageCounter> clientStatistic;

        public MessageCounterService()
        {
            clientStatistic = new Dictionary<long, MessageCounter>();
        }

        private MessageCounter getMessageCounter(long apiId)
        {
            if (!clientStatistic.ContainsKey(apiId))
            {
                clientStatistic.Add(apiId, new MessageCounter());
            }
            return clientStatistic[apiId];
        }
        public bool checkLimitExceeded(long apiId)
        {
            MessageCounter messageCounter = getMessageCounter(apiId);
            messageCounter.count();
            return messageCounter.checkLimitExceeded();
        }

        public int getLimit(long apiId)
        {
            return getMessageCounter(apiId).limit;
        }

        public PostResult setLimit(long apiId, int limit)
        {
            if (!clientStatistic.ContainsKey(apiId))
            {
                return PostResultService.getErrorPostResult(apiId, "Client with apiId not found:" + apiId);
            }
            clientStatistic[apiId].limit = limit;
            return PostResultService.getOkPostResult(apiId, "Limit " + limit + " setup successfully for:" + apiId);
        }

        public Dictionary<long, MessageCounter> getClientStatistic()
        {
            return clientStatistic;
        }

        public PostResult resetCounter(long apiId)
        {
            if (!clientStatistic.ContainsKey(apiId))
            {
                return PostResultService.getErrorPostResult(apiId, "Client with apiId not found:" + apiId);
            }
            int lastCounterValue = clientStatistic[apiId].counter;
            clientStatistic[apiId].resetCounter();
            return PostResultService.getOkPostResult(apiId, "Counter " + lastCounterValue + " ==> 0 reset successfully for:" + apiId);
        }
    }
}
