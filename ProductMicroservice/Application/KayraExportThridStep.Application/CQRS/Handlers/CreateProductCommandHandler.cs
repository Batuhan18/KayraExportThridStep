using KayraExportThridStep.Application.CQRS.Commands;
using KayraExportThridStep.Application.Interfaces;
using KayraExportThridStep.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KayraExportThridStep.Application.CQRS.Handlers
{
    public class CreateProductCommandHandler
    {
        private readonly IRepository<Product> _repository;

        public CreateProductCommandHandler(IRepository<Product> repository)
        {
            _repository = repository;
        }

        public async Task Handle(CreateProductCommand createProductCommand)
        {
            var values = new Product
            {
                ProductImageUrl = createProductCommand.ProductImageUrl,
                ProductName = createProductCommand.ProductName,
                ProductPrice = createProductCommand.ProductPrice
            };

            await _repository.CreateAsync(values);
        }
    }
}
