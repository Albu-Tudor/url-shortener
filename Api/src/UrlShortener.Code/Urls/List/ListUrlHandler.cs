using UrlShortener.Core.Urls.Add;

namespace UrlShortener.Core.Urls.List
{
    public class ListUrlHandler
    {

        private readonly IUserUrlsReader _userUrlsReader;

        public ListUrlHandler(IUserUrlsReader userUrlsReader)
        {
            _userUrlsReader = userUrlsReader;
        }

        public async Task<ListUrlsResponse> HandleAsync(ListUrlsRequest request, CancellationToken cancellationToken)
        {
            return await _userUrlsReader.GetAsync(request.Author, cancellationToken);
        }
    }
}
