using AutoMapper;
using SpendingWeb.DTOs;
using SpendingWeb.Models;
using SpendingWeb.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpendingWeb.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllAsync()
        {
            var categories = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
        }

        public async Task<CategoryDTO?> GetByIdAsync(int id)
        {
            var category = await _repo.GetByIdAsync(id);
            return category == null ? null : _mapper.Map<CategoryDTO>(category);
        }

        public async Task<CategoryDTO> CreateAsync(CategoryDTO dto)
        {
            var category = _mapper.Map<Category>(dto);
            await _repo.AddAsync(category);
            await _repo.SaveAsync();
            return _mapper.Map<CategoryDTO>(category);
        }

        public async Task<bool> UpdateAsync(int id, CategoryDTO dto)
        {
            var category = await _repo.GetByIdAsync(id);
            if (category == null) return false;
            _mapper.Map(dto, category);
            _repo.Update(category);
            await _repo.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _repo.GetByIdAsync(id);
            if (category == null) return false;
            _repo.Remove(category);
            await _repo.SaveAsync();
            return true;
        }
    }
}