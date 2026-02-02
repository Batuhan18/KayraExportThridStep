using KayraExportThridStep.Application.CQRS.Commands;
using KayraExportThridStep.Application.Interfaces;
using KayraExportThridStep.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace KayraExportThridStep.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IRepository<Product> _repository;
        private readonly IProductCommandRepository _productCommandRepository;
        private readonly IMediator _mediator;

        public ProductsController(IRepository<Product> repository, IMediator mediator, IProductCommandRepository productCommandRepository)
        {
            _repository = repository;
            _mediator = mediator;
            _productCommandRepository = productCommandRepository;
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductCommand create)
        {
            try
            {
                Log.Information("Ürün ekle: {@Product}", create);
                var values = await _mediator.Send(create);
                Log.Information("Ürün başarıyla eklendi: {ProductId}", values);
                return Ok(values);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Ürün eklenirken hata oluştu: {@Product}", create);
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateProduct(UpdateProductCommand update)
        {
            await _mediator.Send(update);
            return Ok("Ürün başarıyla güncellendi.");
        }
        [AllowAnonymous]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            await _productCommandRepository.DeleteAsync(id);
            return Ok("Ürün başarıyla silindi.");
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var values = await _repository.GetAllAsync();
            return Ok(values);
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdProduct(int id)
        {
            var values = await _repository.GetByIdAsync(id);
            if (values == null)
            {
                return NotFound();
            }
            return Ok(values);
        }
    }
}
