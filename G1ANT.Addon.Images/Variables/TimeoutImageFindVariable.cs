using G1ANT.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G1ANT.Addon.Core
{
    [Variable(
        Name = "timeoutimagefind",
        Tooltip = "Determines the timeout value (in ms) for the image.find and waifor.image commands; the default value is 20000 (20 seconds)")]
    public class TimeoutImageFindVariable : Variable
    {
        private TimeSpanStructure value;
        public TimeoutImageFindVariable(AbstractScripter scripter = null) : base(scripter)
        {
            value = new TimeSpanStructure(new TimeSpan(0, 0, 20), "", scripter);
        }

        public override Structure GetValue(string index = "")
        {
            return value.Get(index);
        }

        public override void SetValue(Structure value, string index = "")
        {
            this.value.Set(value, index);
        }
    }
}
