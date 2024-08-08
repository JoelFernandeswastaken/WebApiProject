using MediatR;
using Organization.Application.Common.DTO.Request;
using Organization.Application.Common.Exceptions;
using Organization.Application.Common.Interfaces.Persistance;
using Organization.Domain.Company.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Organization.Domain.Common.Errors;
using ErrorOr;

namespace Organization.Application.CompanyModule.Commands.UpdateCompany
{
    public record class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand, ErrorOr<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateCompanyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<bool>> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            var requiredCompany = await _unitOfWork.Companies.GetByIdAsync(request.Id);
            if (requiredCompany == null || requiredCompany.Id != request.Id)
            {
                //  throw new CompanyNotFoundException("Cound not find company with given ID");
                return Errors.Company.CompanyDoesNotExist($"Cound not find company with gived ID: {request.Id}");
            }


            requiredCompany.Name = request.Name;
            requiredCompany.Address = request.Address;
            requiredCompany.Country = request.Country;

            _unitOfWork.BeginTransaction();
            bool result = await _unitOfWork.Companies.UpdateAsync(requiredCompany);
            _unitOfWork.CommitAndCloseConnection();
            if (result) return true;

            return Errors.UnexpectedErrors.InteralServerError("Something went wrong");
        }
    }
}
