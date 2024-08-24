namespace Organization.Presentation.Api.Common.Exceptions
{
    public static class RFC7231Mapper
    {
        private static readonly Dictionary<int, string> StatusCodeToSectionMap = new Dictionary<int, string>
        {
            { 400, "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1" },
            { 401, "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1" },
            { 403, "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3" },
            { 404, "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4" },
            { 405, "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.5" },
            { 406, "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.6" },
            { 500, "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1" },
        };

        public static string GetRFC7231Link(int? statusCode)
        {
            if (statusCode.HasValue && StatusCodeToSectionMap.TryGetValue(statusCode.Value, out var url))
            {
                return url;
            }

            // Default if section not found
            return "https://datatracker.ietf.org/doc/html/rfc7231";
        }
    }

}
