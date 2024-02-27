using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Prestarter.Helpers;
internal static class IconResource
{
    public static Icon ExtractIcon()
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("favicon.ico");

        return new Icon(stream);
    }
}
