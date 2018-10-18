using G1ANT.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G1ANT.Addon.Core
{
    [Variable(
        Name = "timeoutimageexpected",
        Tooltip = "Determines the timeout value for the image.expected command; the default value is 60000 ms.")]
    public class TimeoutImageExpectedVariable : Variable
    {
        private TimeSpanStructure value;
        public TimeoutImageExpectedVariable(AbstractScripter scripter = null) : base(scripter)
        {
            value = new TimeSpanStructure(new TimeSpan(0, 0, 60), "", scripter);
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
