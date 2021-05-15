using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsInfoCat.Test.ComponentSupport
{
    public class SelfComparableExample : IEquatable<SelfComparableExample>, IComparable<SelfComparableExample>
    {
        public int CompareTo(SelfComparableExample other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(SelfComparableExample other)
        {
            throw new NotImplementedException();
        }
    }
    public class SelfComparableExample<T> : SelfComparableExample
        where T : IEquatable<SelfComparableExample>, IComparable<SelfComparableExample>
    {
    }

}
