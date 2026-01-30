using KayraExportThridStep.Application.CQRS.Queries;
using KayraExportThridStep.Application.CQRS.Results;
using KayraExportThridStep.Application.Interfaces;
using KayraExportThridStep.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KayraExportThridStep.Application.CQRS.Handlers
{
    public class GetProductByIdQueryHandler
    {
        private readonly IRepository<Product> _repository;

        public GetProductByIdQueryHandler(IRepository<Product> repository)
        {
            _repository = repository;
        }

        public async Task<GetProductByIdQueryResult> Handle(GetProductByIdQuery query)
        {
            var values = await _repository.GetByIdAsync(query.Id);
            return new GetProductByIdQueryResult
            {
                ProductId = values.ProductId,
                ProductImageUrl = values.ProductImageUrl,
                ProductName = values.ProductName,
                ProductPrice = values.ProductPrice
            };
        }
    }
}
