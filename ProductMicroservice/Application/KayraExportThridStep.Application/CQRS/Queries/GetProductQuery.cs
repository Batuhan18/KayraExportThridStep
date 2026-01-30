using KayraExportThridStep.Application.CQRS.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KayraExportThridStep.Application.CQRS.Queries
{
    public class GetProductQuery : IRequest<List<GetProductQueryResult>>
    {
    }
}
