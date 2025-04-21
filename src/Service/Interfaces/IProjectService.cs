using Service.Dtos;
using Service.Models;

namespace Service.Interfaces;
public interface IProjectService : IService<ProjectModel, ProjectDto>
{
    public Task<bool> CloseProjectAsync(Guid id);
    public Task<bool> StartProjectAsync(Guid id);
    public Task<bool> UpdateMemberList(List<Guid> memberIds, Guid projectId);
}
