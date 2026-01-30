using KayraExportThridStep.Application.CQRS.Queries;
using KayraExportThridStep.Application.CQRS.Results;
using KayraExportThridStep.Application.CQRS.Service;
using KayraExportThridStep.Application.Interfaces;
using KayraExportThridStep.Core.Entities;
using MediatR;

namespace KayraExportThridStep.Application.CQRS.Handlers
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, List<GetProductQueryResult>>
    {
        private readonly IRepository<Product> _repository;
        private readonly ICacheService _cache;

        public GetProductQueryHandler(IRepository<Product> repository, ICacheService cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<List<GetProductQueryResult>> Handle(
            GetProductQuery request,
            CancellationToken cancellationToken)
        {
            var cacheKey = "products_all";

            var cached =
                await _cache.GetAsync<List<GetProductQueryResult>>(cacheKey);

            if (cached != null)
                return cached;

            var products = await _repository.GetAllAsync();

            var values = products.Select(x => new GetProductQueryResult
            {
                ProductId = x.ProductId,
                ProductImageUrl = x.ProductImageUrl,
                ProductName = x.ProductName,
                ProductPrice = x.ProductPrice
            }).ToList();

            await _cache.SetAsync(cacheKey, values, TimeSpan.FromMinutes(5));

            return values;
        }
    }
}
