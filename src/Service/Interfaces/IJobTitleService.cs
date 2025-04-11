

using Service.Dtos;
using Service.Models;

namespace Service.Interfaces;

public interface IJobTitleService : IService<JobTitleModel, JobTitleDto>
{
    public Task<JobTitleModel> GetByTitleNameAsync(string titleName);
    public Task<bool> Exists(string titleName);

}
