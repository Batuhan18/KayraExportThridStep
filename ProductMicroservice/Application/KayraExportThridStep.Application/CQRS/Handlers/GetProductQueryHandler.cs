using KayraExportThridStep.Application.CQRS.Service;
using KayraExportThridStep.Application.Dtos.Product;
using KayraExportThridStep.Application.Interfaces;
using KayraExportThridStep.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KayraExportThridStep.Application.CQRS.Handlers
{
    public class GetProductQueryHandler
    {
        private readonly IRepository<Product> _repository;
        private readonly ICacheService _cache;

        public GetProductQueryHandler(IRepository<Product> repository, ICacheService cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<List<ResultProductDto>> Handle()
        {
            var cacheKey = "products_all";
            var cached = await _cache.GetAsync<List<ResultProductDto>>(cacheKey);
            if (cached != null)
                return cached;

            var products = await _repository.GetAllAsync();

            var values = products.Select(x => new ResultProductDto
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
