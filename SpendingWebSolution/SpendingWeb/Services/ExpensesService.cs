using AutoMapper;
using SpendingWeb.DTOs;
using SpendingWeb.Models;
using SpendingWeb.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpendingWeb.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _repo;
        private readonly IMapper _mapper;

        public ExpenseService(IExpenseRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ExpenseDTO>> GetAllAsync()
        {
            var expenses = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<ExpenseDTO>>(expenses);
        }

        public async Task<ExpenseDTO?> GetByIdAsync(int id)
        {
            var expense = await _repo.GetByIdAsync(id);
            return expense == null ? null : _mapper.Map<ExpenseDTO>(expense);
        }

        public async Task<ExpenseDTO> CreateAsync(CreateExpenseDTO dto)
        {
            var expense = _mapper.Map<Expense>(dto);
            await _repo.AddAsync(expense);
            await _repo.SaveAsync();
            return _mapper.Map<ExpenseDTO>(expense);
        }

        public async Task<bool> UpdateAsync(int id, ExpenseDTO dto)
        {
            var expense = await _repo.GetByIdAsync(id);
            if (expense == null) return false;
            _mapper.Map(dto, expense);
            _repo.Update(expense);
            await _repo.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var expense = await _repo.GetByIdAsync(id);
            if (expense == null) return false;
            _repo.Remove(expense);
            await _repo.SaveAsync();
            return true;
        }
    }
}