using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Udp.Attributes {
    public class ExistsPrefixAttribute : Attribute {
        public Type ExistsType;
        public object TrueValue;

        public ExistsPrefixAttribute( Type t, object trueVal ) {
            ExistsType = t;
            TrueValue = trueVal;
        }
    }
}
