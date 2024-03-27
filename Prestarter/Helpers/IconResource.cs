using System.Drawing;
using System.Reflection;

namespace Prestarter.Helpers;
internal static class IconResource
{
    public static Icon ExtractIcon()
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("favicon.ico");

        return new Icon(stream);
    }
}
