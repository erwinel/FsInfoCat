using System;
using System.Collections;
using System.Collections.Generic;

namespace FsInfoCat.Collections
{
    public class GeneralizableComparer<T> : IGeneralizableComparer<T>
    {
        public static readonly IGeneralizableComparer<T> Default;
        private readonly IComparer<T> _typedComparer;
        private readonly IComparer _genericComparer;

        static GeneralizableComparer()
        {
            Type type = typeof(T);
            if (type.Equals(typeof(string)))
                Default = (IGeneralizableComparer<T>)Activator.CreateInstance(typeof(GeneralizableComparer<>).MakeGenericType(type), new object[] { StringComparer.InvariantCulture });
            else
                Default = (IGeneralizableComparer<T>)Activator.CreateInstance(typeof(GeneralizableComparer<>).MakeGenericType(type), new object[] { Comparer<T>.Default });
        }

        public GeneralizableComparer() : this((IComparer<T>)null) { }

        public GeneralizableComparer(IComparer comparer)
        {
            if (comparer is null)
            {
                _typedComparer = Comparer<T>.Default;
                _genericComparer = new CoersionComparer(_typedComparer);
            }
            else
            {
                _genericComparer = comparer;
                if (comparer is IComparer<T> typedComparer)
                    _typedComparer = typedComparer;
                else
                    _typedComparer = Comparer<T>.Default;
            }
        }

        public GeneralizableComparer(IComparer<T> comparer)
        {
            _typedComparer = comparer ?? Comparer<T>.Default;
            _genericComparer = (comparer is IComparer g) ? g : new CoersionComparer(_typedComparer);
        }

        public int Compare(T x, T y) => _typedComparer.Compare(x, y);

        int IComparer.Compare(object x, object y) => _genericComparer.Compare(x, y);

        private class CoersionComparer : IComparer
        {
            private readonly IComparer<T> _typedComparer;

            internal CoersionComparer(IComparer<T> typedComparer) { _typedComparer = typedComparer; }

            public int Compare(object x, object y)
            {
                if (Coersion<T>.Default.TryCoerce(x, out T a))
                {
                    if (Coersion<T>.Default.TryCoerce(y, out T b))
                        return _typedComparer.Compare(a, b);
                    return -1;
                }
                if (Coersion<T>.Default.TryCoerce(y, out _))
                    return 1;
                return Comparer.Default.Compare(x, y);
            }
        }
    }
}
