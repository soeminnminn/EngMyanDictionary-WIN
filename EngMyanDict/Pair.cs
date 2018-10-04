using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngMyanDict
{
    public class Pair<T1, T2> : IComparable<Pair<T1, T2>>
    {
        #region Variables
        private T1 mFirst;
        private T2 mSecond;
        #endregion

        #region Constructor
        public Pair()
        {

        }

        public Pair(T1 first, T2 second)
        {
            this.mFirst = first;
            this.mSecond = second;
        }
        #endregion

        #region Methods
        public static Pair<T1, T2> Create(T1 first, T2 second)
        {
            return new Pair<T1, T2>(first, second);
        }

        public int CompareTo(Pair<T1, T2> other)
        {
            if (other == null) return 1;
            int comp = Comparer<T1>.Default.Compare(this.First, other.First);
            if (comp != 0) return comp;
            return Comparer<T2>.Default.Compare(this.Second, other.Second);
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder(16);
            stringBuilder.Append('[');
            if (this.First != null)
            {
                stringBuilder.Append(this.First.ToString());
            }
            stringBuilder.Append(", ");
            if (this.Second != null)
            {
                stringBuilder.Append(this.Second.ToString());
            }
            stringBuilder.Append(']');
            return stringBuilder.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return EqualityComparer<object>.Default.Equals(this, obj);
        }

        public override int GetHashCode()
        {
            int h1 = this.First != null ? this.First.GetHashCode() : 0;
            int h2 = this.Second != null ? this.Second.GetHashCode() : 0;
            return (h1 << 5) + h1 ^ h2;
        }
        #endregion

        #region Properties
        public T1 First
        {
            get { return this.mFirst; }
            set { this.mFirst = value; }
        }

        public T2 Second
        {
            get { return this.mSecond; }
            set { this.mSecond = value; }
        }
        #endregion
    }
}
