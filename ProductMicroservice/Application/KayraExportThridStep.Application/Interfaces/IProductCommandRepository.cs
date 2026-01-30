using KayraExportThridStep.Application.CQRS.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KayraExportThridStep.Application.Interfaces
{
    public interface IProductCommandRepository
    {
        Task<int> CreateAsync(CreateProductCommand command);
        Task UpdateAsync(UpdateProductCommand command);
        Task DeleteAsync(int id);
    }
}
