using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using G1ANT.Language;

namespace G1ANT.Addon.Images
{
    [Addon(Name = "Images",
        Tooltip = "images Commands")]
    [CommandGroup(Name = "image", Tooltip = "Command connected with creating, editing and generally working on images")]
    [CommandGroup(Name = "waitfor", Tooltip = "Command connected with creating, editing and generally working on images")]
    public class ImagesAddon : Language.Addon
    {
    }
}