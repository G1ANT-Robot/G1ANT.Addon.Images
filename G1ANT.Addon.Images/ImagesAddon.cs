/**
*    Copyright(C) G1ANT Ltd, All rights reserved
*    Solution G1ANT.Addon, Project G1ANT.Addon.Images
*    www.g1ant.com
*
*    Licensed under the G1ANT license.
*    See License.txt file in the project root for full license information.
*
*/
using G1ANT.Language;

namespace G1ANT.Addon.Images
{
    [Addon(Name = "Images", Tooltip = "images Commands")]
    [Copyright(Author = "G1ANT LTD", Copyright = "G1ANT LTD", Email = "hi@g1ant.com", Website = "www.g1ant.com")]
    [License(Type = "LGPL", ResourceName = "License.txt")]
    [CommandGroup(Name = "image", Tooltip = "Command connected with creating, editing and generally working on images")]
    [CommandGroup(Name = "waitfor", Tooltip = "Command connected with creating, editing and generally working on images")]
    public class ImagesAddon : Language.Addon
    {
    }
}