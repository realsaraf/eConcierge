using Infrasturcture.TouchLibrary;

namespace TouchHelpers.Helper
{
    public interface IShareHelper
    {
        void UploadMedia(MediaType mediaType, string path);
    }
}