using SpendingWeb.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpendingWeb.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDTO>> GetAllAsync();
        Task<CategoryDTO?> GetByIdAsync(int id);
        Task<CategoryDTO> CreateAsync(CategoryDTO dto);
        Task<bool> UpdateAsync(int id, CategoryDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}