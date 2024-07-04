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
                new CompanyResponse()
                {
                    Name = "name1",
                    Address = "address1",
                    Country = "country1"
                },
                new CompanyResponse()
                {
                    Name = "name2",
                    Address = "address2",
                    Country = "country2"
                }
            };
            return PageList<CompanyResponse>.Create(companiesList, 1, 100, 100);
        }
    }
}
