using UrlShortener.Core.Urls.Add;

namespace UrlShortener.Core.Urls.List
{
    public class ListUrlHandler
    {

        private readonly IUserUrlsReader _userUrlsReader;
        private const int MaxPageSize = 20;

        public ListUrlHandler(IUserUrlsReader userUrlsReader)
        {
            _userUrlsReader = userUrlsReader;
        }

        public async Task<ListUrlsResponse> HandleAsync(ListUrlsRequest request, CancellationToken cancellationToken)
        {
            return await _userUrlsReader.GetAsync(request.Author,
                int.Min(request.pageSize ?? MaxPageSize, MaxPageSize),
                request.continuationToken,
                cancellationToken);
        }
    }
}
