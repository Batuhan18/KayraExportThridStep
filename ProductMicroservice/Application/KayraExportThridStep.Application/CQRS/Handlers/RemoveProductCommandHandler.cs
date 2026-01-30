using KayraExportThridStep.Application.CQRS.Commands;
using KayraExportThridStep.Application.Interfaces;
using MediatR;

namespace KayraExportThridStep.Application.CQRS.Handlers
{
    public class RemoveProductCommandHandler
        : IRequestHandler<RemoveProductCommand, Unit>
    {
        private readonly IProductCommandRepository _commandRepository;

        public RemoveProductCommandHandler(IProductCommandRepository commandRepository)
        {
            _commandRepository = commandRepository;
        }

        public async Task<Unit> Handle(RemoveProductCommand request,CancellationToken cancellationToken)
        {
            await _commandRepository.DeleteAsync(request.Id);
            return Unit.Value;
        }
    }
}
