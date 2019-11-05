using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace G1ANT.Addon.Images
{
    public static class AddonExtension
    {
        public static void UnpackAssembly(this Language.Addon addon, string name, string version = null)
        {
            try
            {
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                if (!string.IsNullOrEmpty(version))
                    path = Path.Combine(path, version);

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var fullPath = Path.Combine(path, name);

                if (!File.Exists(fullPath))
                {
                    var resourceName = Assembly.GetExecutingAssembly().GetManifestResourceNames().Where(res => res.Contains(version == null ? name : $"{version}.{name}")).FirstOrDefault();

                    if (!string.IsNullOrEmpty(resourceName))
                    {
                        using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                        using (var file = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                        {
                            resource.CopyTo(file);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not unpack the resource '{name}'. Message: {ex.Message}", ex);
            }
        }
    }
}
