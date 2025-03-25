using Service.Interfaces;
using Ui.Asp.Mvc.Controllers;

namespace Ui.Asp.Mvc.Services;

public class ImageManager(IWebHostEnvironment env, IConfiguration config)
{
    private readonly IWebHostEnvironment _env = env;
    private readonly IConfiguration _config = config;

    public void SaveImage(IFormFile file)
    {
        var projectAvatarFolder = _config.GetSection("ProjectAvatarFolder").Value;
    }
    
}
