using SpendingWeb.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpendingWeb.Services
{
    public interface IExpenseService
    {
        Task<IEnumerable<ExpenseDTO>> GetAllAsync();
        Task<ExpenseDTO?> GetByIdAsync(int id);
        Task<ExpenseDTO> CreateAsync(CreateExpenseDTO dto);
        Task<bool> UpdateAsync(int id, ExpenseDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}