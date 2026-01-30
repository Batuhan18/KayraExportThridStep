using KayraExportThridStep.Application.CQRS.Commands;
using KayraExportThridStep.Application.Interfaces;
using MediatR;

namespace KayraExportThridStep.Application.CQRS.Handlers
{
    public class UpdateProductCommandHandler
        : IRequestHandler<UpdateProductCommand, Unit>
    {
        private readonly IProductCommandRepository _commandRepository;

        public UpdateProductCommandHandler(IProductCommandRepository commandRepository)
        {
            _commandRepository = commandRepository;
        }

        public async Task<Unit> Handle(UpdateProductCommand request,CancellationToken cancellationToken)
        {
            await _commandRepository.UpdateAsync(request);
            return Unit.Value;
        }
    }
}
