using AutoMapper;
using SpendingWeb.Models;
using SpendingWeb.DTOs;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Expense <-> ExpenseDTO
        CreateMap<Expense, ExpenseDTO>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
            .ReverseMap();

        // CreateExpenseDTO -> Expense
        CreateMap<CreateExpenseDTO, Expense>();

        // Category <-> CategoryDTO
        CreateMap<Category, CategoryDTO>().ReverseMap();

        // CreateCategoryDTO -> Category
        CreateMap<CreateCategoryDTO, Category>();
    }
}