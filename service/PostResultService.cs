using TelegrammService.model;

namespace TelegrammService.service
{
    public class PostResultService
    {

        public static PostResult getErrorPostResult(int apiId, string description)
        {
            return getPostResult(apiId, description, PostResultCode.ERROR);
        }

        public static PostResult getOkPostResult(int apiId, string description)
        {
            return getPostResult(apiId, description, PostResultCode.OK);
        }

        public static PostResult getPostResult(int apiId, string description, PostResultCode postResultCode)
        {
            PostResult postResult = new PostResult();
            postResult.apiId = apiId;
            postResult.description = description;
            postResult.postResultCode = postResultCode;
            return postResult;
        }
    }
}
