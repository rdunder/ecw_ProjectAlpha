using Service.Models;

namespace Ui.Asp.Mvc.Models;

public class JobTitlesViewModel
{
    public IEnumerable<JobTitleModel> JobTitles { get; set; } = [];
    public JobTitlesFormViewModel Form { get; set; } = new();
}
