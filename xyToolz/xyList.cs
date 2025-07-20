using System;
using System.Buffers;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xyToolz
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    ref struct xyList<T>
    {
        private Span<T> _span;
        private T[]? _pooledArray;
        private int _count;
        private uint _factor;

        public xyList(Span<T> initialBuffer_, uint expansionFactor_ =2)
        {
            _factor = expansionFactor_;
            _span = initialBuffer_;
            _count = 0;
            _pooledArray = null;
        }

        public int Count => _count;
        public int Capacity => _span.Length;


        public void Add( T target)
        {
            if(_count >= _span.Length)
            {
                Expand();
                
            }
            _span[_count++] = target;
        }


        public void RemoveAt(int index)
        {
            if (index < 0 || index >= _count)
            {
                throw new ArgumentOutOfRangeException(paramName: nameof(index));
            }
            else
            {
                // Select the targeted part of the span
                Span<T> tail = _span.Slice(start: index +1,length: _count - index -1);

                // Move over
                tail.CopyTo(_span.Slice(start: index));

                _count--;

                // Delete reference!!!
                _span[_count] = default!;

            }
        }



        /// <summary>
        /// Gets or sets the element at the specified index in the collection.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set. Must be within the range of the collection.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="index"/> is less than 0 or greater than or equal to the number of elements in the
        /// collection.</exception>
        public T this[int index]
        {
            get 
            {
                if (index < 0 || index >= _count) 
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
                else
                {
                    return _span[index];
                }
            }
            set 
            {
                if (index < 0 || index >= _count)
                {
                    throw new ArgumentOutOfRangeException(paramName: nameof(index));
                }
                else
                {
                    _span[index] = value;
                }
            }
        }

        public void Dispose()
        {

            DisposePooledArray();
        }

        public void DisposePooledArray() 
        {
            if (_pooledArray != null)
            {
                ArrayPool<T>.Shared.Return(array: _pooledArray, clearArray: true);
                _pooledArray = default!;
            }
        }

        public void Expand()
        {
            int newSize = Math.Max(val1:4, val2: _span.Length * (int)_factor);
            T[] newPooledArray = ArrayPool<T>.Shared.Rent(minimumLength: newSize);
            _span.Slice(start: 0, length: _count).CopyTo(destination: newPooledArray);

            if (_pooledArray != null)
            {
                ArrayPool<T>.Shared.Return(array: _pooledArray, clearArray: true);
            }
                _pooledArray = newPooledArray;
                _span = newPooledArray;
          

        }


        /// <summary>
        /// Clear the span and reset the counter
        /// </summary>
        public void Clear()
        {
            _span.Slice(start: 0, length: _count).Clear();
            _count = 0;
        }



        public T[] ToArray()
        {
            var result = new T[_count];
            _span.Slice(0,_count).CopyTo(result);
            return result;
        }
    }
}
