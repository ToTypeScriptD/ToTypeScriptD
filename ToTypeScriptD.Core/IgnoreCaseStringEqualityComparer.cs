using System.Collections.Generic;

namespace ToTypeScriptD.Core
{
    public class IgnoreCaseStringEqualityComparer : EqualityComparer<string>
    {

        public override bool Equals(string x, string y)
        {
            if (x == null && y == null) return true;
            if (x == null) return false;
            return x.Equals(y);
        }

        public override int GetHashCode(string obj)
        {
            return obj.GetHashCode();
        }
    }
}
