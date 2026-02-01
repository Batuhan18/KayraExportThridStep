using KayraExportThridStep.Application.CQRS.Commands;
using KayraExportThridStep.Application.CQRS.Service;
using KayraExportThridStep.Application.Interfaces;
using MediatR;

namespace KayraExportThridStep.Application.CQRS.Handlers
{
    public class CreateProductCommandHandler
        : IRequestHandler<CreateProductCommand, int>
    {
        private readonly IProductCommandRepository _productCommandRepository;
        private readonly ICacheService _cache;

        public CreateProductCommandHandler(
            IProductCommandRepository productCommandRepository, ICacheService cache
            )
        {
            _productCommandRepository = productCommandRepository;
            _cache = cache;
        }

        public async Task<int> Handle(
            CreateProductCommand request,
            CancellationToken cancellationToken)
        {
            var id = await _productCommandRepository.CreateAsync(request);

            //cache invalidation
            await _cache.RemoveAsync("products_all");

            return id;
        }
    }
}
