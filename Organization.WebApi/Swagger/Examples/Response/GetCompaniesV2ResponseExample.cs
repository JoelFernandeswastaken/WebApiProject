using Organization.Application.Common.DTO.Response;
using Organization.Application.Common.Utilities;
using Organization.Infrastructure.Persistance.Repositories;
using Swashbuckle.AspNetCore.Filters;

namespace Organization.Presentation.Api.Swagger.Examples.Response
{
    public class GetCompaniesV2ResponseExample : IExamplesProvider<PageList<CompanyResponse>>
    {
        public PageList<CompanyResponse> GetExamples()
        {
            var companiesList = new List<CompanyResponse>()
            {
                new CompanyResponse("id1", "name1", "address1", "country1"),
                new CompanyResponse("id2", "name2", "address2", "country2")
            };
            return PageList<CompanyResponse>.Create(companiesList, 1, 100, 100);
        }
    }
}
