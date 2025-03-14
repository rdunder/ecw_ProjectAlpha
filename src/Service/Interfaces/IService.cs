

namespace Service.Interfaces;

public interface IService<TModel, TDto>
{
    public Task<bool> CreateAsync(TDto? dto);
    public Task<IEnumerable<TModel>> GetAllAsync();
    public Task<TModel> GetByIdAsync(int id);
    public Task<bool> UpdateAsync(int id, TDto? dto);
    public Task<bool> DeleteAsync(int id);
}
