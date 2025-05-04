using Service.Interfaces;
using Ui.Asp.Mvc.Controllers;

namespace Ui.Asp.Mvc.Services;

public class ImageManager(IWebHostEnvironment env, IConfiguration config, ILogger<ImageManager> logger)
{
    private readonly IWebHostEnvironment _env = env;
    private readonly IConfiguration _config = config;
    private readonly ILogger<ImageManager> _logger = logger;

    public async Task<string> SaveImage(IFormFile file, string controller)
    {
        try
        {


            var uploadFolder = Path.Combine(_env.WebRootPath, $"images/{controller.Replace("Controller", "")}_Avatars");
            Directory.CreateDirectory(uploadFolder);

            var newFileName = $"{controller.Replace("Controller", "")}_Avatar_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadFolder, newFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return newFileName;
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"ERROR when saving image: {ex.Message}\nWebRoot Path: {_env.WebRootPath}");
            return null!;
        }
    }
    
    public void DeleteImage(string avatar, string controller)
    {
        if (avatar == null) return;

        if (avatar.StartsWith(nameof(controller)))
        {
            var filePath = Path.Combine(
            _env.WebRootPath,
            $"images{Path.DirectorySeparatorChar}{controller.Replace("Controller", "")}_Avatars{Path.DirectorySeparatorChar}{avatar}");
            File.Delete(filePath);
        }
        
    }

    public string GetPath(string controller, string avatar)
    {
        string controllerName = controller.Replace("Controller", "");

        if (avatar is null) return $"/images/{controllerName}_DefaultAvatar.png";

        if (avatar.StartsWith("https:")) return avatar;


        var path = $"/images/{controllerName}_Avatars/{avatar}";
        return path ;
    }
}
