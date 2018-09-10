using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPlayGround.Comparer
{
    public class Citizen : IComparable<Citizen>
    {
        // Implement the generic CompareTo method with the Temperature 
        // class as the Type parameter. 
        //
        public int CompareTo(Citizen other)
        {
            // If other is not a valid object reference, this instance is greater.
            if (other == null) return 1;

            if (this > other)
                return 1;
            else if (this < other)
                return -1;
            else
                return 0;            
        }

        // Define the is greater than operator.
        public static bool operator >(Citizen c1, Citizen c2)
        {            
            return (c1.Deposit + c1.Cash - c1.Mortgage).CompareTo(c2.Deposit + c2.Cash - c2.Mortgage) == 1;
        }

        // Define the is less than operator.
        public static bool operator <(Citizen c1, Citizen c2)
        {
            return (c1.Deposit + c1.Cash - c1.Mortgage).CompareTo(c2.Deposit + c2.Cash - c2.Mortgage) == -1;
        }

        // Define the is greater than or equal to operator.
        public static bool operator >=(Citizen c1, Citizen c2)
        {
            return (c1.Deposit + c1.Cash - c1.Mortgage).CompareTo(c2.Deposit + c2.Cash - c2.Mortgage) >= 0;
        }

        // Define the is less than or equal to operator.
        public static bool operator <=(Citizen c1, Citizen c2)
        {
            return (c1.Deposit + c1.Cash - c1.Mortgage).CompareTo(c2.Deposit + c2.Cash - c2.Mortgage) <= 0;
        }

        
        public Citizen(int Age, int Deposit, int Cash, int Mortgage)
        {
            this.Age = Age;
            this.Deposit = Deposit;
            this.Cash = Cash;
            this.Mortgage = Mortgage;

        }

        public int Age { get; set; }

        public int Deposit { get; set; }

        public int Cash { get; set; }

        public int Mortgage { get; set; }

        public int Health { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPlayGround.Comparer
{
    public class DepositComparer : IComparer<Citizen>
    {
        // Compares by Height, Length, and Width.
        public int Compare(Citizen x, Citizen y)
        {
            if (x.Deposit.CompareTo(y.Deposit) != 0)
            {
                return x.Deposit.CompareTo(y.Deposit);
            }
            else if (x.Cash.CompareTo(y.Cash) != 0)
            {
                return x.Cash.CompareTo(y.Cash);
            }
            else if (x.Mortgage.CompareTo(y.Mortgage) != 0)
            {
                return x.Mortgage.CompareTo(y.Mortgage)*-1;
            }
            else
            {
                return 0;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPlayGround.Comparer_Decorate
{
    public class SalaryComparer : IComparer<Person>
    {
        int IComparer<Person>.Compare(Person x, Person y)
        {
            return x.Salary.CompareTo(y.Salary);
        }
    }
    public class NameComparer : IComparer<Person>
    {
        int IComparer<Person>.Compare(Person x, Person y)
        {
            return x.firstName.CompareTo(y.firstName);
        }
    }
    public class AgeComparer : IComparer<Person>
    {
        int IComparer<Person>.Compare(Person x, Person y)
        {
            return x.Age.CompareTo(y.Age);
        }
    }
    public class ComboComparer<Person> : IComparer<Person>
    {
        IComparer<Person> comparer1;
        IComparer<Person> comparer2;
        public ComboComparer(IComparer<Person> comparer1, IComparer<Person> comparer2)
        {
            this.comparer1 = comparer1;
            this.comparer2 = comparer2;
        }

        int IComparer<Person>.Compare(Person x, Person y)
        {
            var result = comparer1.Compare(x, y);
            if (result != 0)
                return result;
            return this.comparer2.Compare(x, y);
        }        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPlayGround.Interface
{
    public class InterfaceTests : IComparable<Person>
    {
        public int CompareTo(Person other)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPlayGround.JonSkeets
{
    public class OrderedEnumerable<TElement> : IOrderedEnumerable<TElement>
    {
        private readonly IEnumerable<TElement> source;
        private readonly IComparer<TElement> currentComparer;

        internal OrderedEnumerable(IEnumerable<TElement> source,
            IComparer<TElement> comparer)
        {
            this.source = source;
            this.currentComparer = comparer;
        }

        public IOrderedEnumerable<TElement> CreateOrderedEnumerable<TKey>
            (Func<TElement, TKey> keySelector,
             IComparer<TKey> comparer,
             bool descending)
        {
            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }

            IComparer<TElement> secondaryComparer =
                new ProjectionComparer<TElement, TKey>(keySelector, comparer);

            if (descending)
            {
                secondaryComparer = new ReverseComparer<TElement>(secondaryComparer);
            }

            return new OrderedEnumerable<TElement>(source,
                new CompoundComparer<TElement>(currentComparer, secondaryComparer));
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            List<TElement> elements = this.source.ToList();

            while(elements.Count > 0)
            {
                TElement minElement = elements[0];
                int minIndex = 0;
                for(int i = 1; i <elements.Count; i++)
                {
                    if(this.currentComparer.Compare(elements[i], minElement) < 0)
                    {
                        minElement = elements[i];
                        minIndex = i;
                    }                   
                }
                elements.RemoveAt(minIndex);
                yield return minElement;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPlayGround.JonSkeets
{
    internal class ProjectionComparer<TElement, TKey> : IComparer<TElement>
    {
        private readonly Func<TElement, TKey> keySelector;
        private readonly IComparer<TKey> comparer;
        internal ProjectionComparer(Func<TElement, TKey> keySelector,
            IComparer<TKey> comparer)
        {
            this.keySelector = keySelector;
            this.comparer = comparer ?? Comparer<TKey>.Default;
        }

        public int Compare(TElement x, TElement y)
        {
            TKey keyX = keySelector(x);
            TKey keyY = keySelector(y);
            return comparer.Compare(keyX, keyY);
        }
    }

    internal class ReverseComparer<T> : IComparer<T>
    {
        private readonly IComparer<T> forwardComparer;
        internal ReverseComparer(IComparer<T> forwardComparer)
        {
            this.forwardComparer = forwardComparer;
        }

        public int Compare(T x, T y)
        {
            return forwardComparer.Compare(y, x);
        }
    }

    internal class CompoundComparer<T> : IComparer<T>
    {
        private readonly IComparer<T> primary;
        private readonly IComparer<T> secondary;
        internal CompoundComparer(IComparer<T> primary,
            IComparer<T> secondary)
        {
            this.primary = primary;
            this.secondary = secondary;
        }

        public int Compare(T x, T y)
        {
            int primaryResult = primary.Compare(x, y);
            if (primaryResult != 0)
            {
                return primaryResult;
            }
            return secondary.Compare(x, y);
        }
    }

}





