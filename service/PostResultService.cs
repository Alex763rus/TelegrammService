using TelegrammService.model;

namespace TelegrammService.service
{
    public class PostResultService
    {

        public static PostResult getErrorPostResult(long apiId, string description)
        {
            return getPostResult(apiId, description, PostResultCode.ERROR);
        }

        public static PostResult getOkPostResult(long apiId, string description)
        {
            return getPostResult(apiId, description, PostResultCode.OK);
        }

        public static PostResult getOkPostResult(string description)
        {
            return getOkPostResult(0, description);
        }

        public static PostResult getPostResult(long apiId, string description, PostResultCode postResultCode)
        {
            PostResult postResult = new PostResult();
            postResult.apiId = apiId;
            postResult.description = description;
            postResult.postResultCode = postResultCode;
            return postResult;
        }
    }
}
