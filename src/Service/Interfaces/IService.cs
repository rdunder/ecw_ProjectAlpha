

namespace Service.Interfaces;

public interface IService<TModel, TDto>
{
    public Task<bool> CreateAsync(TDto? dto);
    public Task<IEnumerable<TModel>> GetAllAsync();
    public Task<TModel> GetByIdAsync(Guid id);
    public Task<bool> UpdateAsync(Guid id, TDto? dto);
    public Task<bool> DeleteAsync(Guid id);
}
