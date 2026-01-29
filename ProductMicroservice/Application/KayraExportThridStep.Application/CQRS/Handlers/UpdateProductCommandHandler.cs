using KayraExportThridStep.Application.CQRS.Commands;
using KayraExportThridStep.Application.Interfaces;
using KayraExportThridStep.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KayraExportThridStep.Application.CQRS.Handlers
{
    public class UpdateProductCommandHandler
    {
        private readonly IRepository<Product> _repository;

        public UpdateProductCommandHandler(IRepository<Product> repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateProductCommand command)
        {
            var values = await _repository.GetByIdAsync(command.ProductId);
            values.ProductName = command.ProductName;
            values.ProductPrice = command.ProductPrice;
            values.ProductImageUrl = command.ProductImageUrl;
            await _repository.UpdateAsync(values);
        }
    }
}
