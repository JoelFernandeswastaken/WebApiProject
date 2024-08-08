using MediatR;
using Organization.Application.Common.Exceptions;
using Organization.Application.Common.Interfaces.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErrorOr;
using Organization.Domain.Common.Errors;

namespace Organization.Application.CompanyModule.Commands.DeleteCompany
{
    public class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand, ErrorOr<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteCompanyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ErrorOr<int>> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
        {
            var deleteCompany = await _unitOfWork.Companies.GetByIdAsync(request.id);
            if (deleteCompany == null || deleteCompany.Id != request.id)
            {
                return Errors.Company.CompanyDoesNotExist($"Company with the id {request.id} does not exist");
                // throw new CompanyNotFoundException("Could not find company with given ID");
            }

            _unitOfWork.BeginTransaction();
            int rowsAffected = await _unitOfWork.Companies.SoftDeleteAsync(request.id, request.deleteAssociations);
            _unitOfWork.CommitAndCloseConnection();

            if (rowsAffected >= 0) return rowsAffected;

            return Errors.UnexpectedErrors.InteralServerError();
        }
    }
}
