using KayraExportThridStep.Application.CQRS.Commands;
using KayraExportThridStep.Application.Interfaces;
using KayraExportThridStep.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KayraExportThridStep.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IRepository<Product> _repository;
        private readonly IMediator _mediator;

        public ProductsController(IRepository<Product> repository, IMediator mediator)
        {
            _repository = repository;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductCommand create)
        {
            var values = await _mediator.Send(create);
            return Ok(values);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(UpdateProductCommand update)
        {
            await _mediator.Send(update);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> GetProduct()
        {
            var values = await _repository.GetAllAsync();
            return Ok(values);
        }

        [HttpGet]
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
