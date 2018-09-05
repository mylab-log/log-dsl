using System.Collections;
using System.Collections.Generic;
using MyLab.Logging;

namespace MyLab.LogDsl
{
    class ConditionsLogEntityAttribute : LogEntityAttribute
    {
        public ConditionsLogEntityAttribute(IEnumerable<string> conditions) 
            : base(LogAttributeNames.ConditionsAttributeName, string.Join(", ", conditions))
        {
        }
    }
}
