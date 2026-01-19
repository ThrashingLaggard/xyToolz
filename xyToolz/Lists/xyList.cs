using System;
using System.Buffers;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xyToolz.Lists
{


    //                                                                                                                                     Disclaimer: this is just an experiment for learning purposes, please neither shout at me nor hit me 





    /// <summary>
    /// Sits on the stack and can NOT go on the heap!
    /// This can neither be used in async/await, iterators, interfaces nor in fields 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public ref struct xyList<T>
    {
        // <summary>
        // This is a thread-safe resource pool that enables reusing instances of type T[].
        // Rent and return the buffers for better performance
        // </summary>
        //privat e Arra yPoo l pool
     
        /// <summary>
        /// Initializes a new instance of the <see cref="xyList{T}"/> class with the specified initial buffer and
        /// expansion factor.
        /// </summary>
        /// <remarks>The <see cref="xyList{T}"/> class uses the provided <paramref name="initialBuffer_"/>
        /// as its initial storage. If the buffer's capacity is exceeded, a new buffer will be allocated with a size
        /// determined by the <paramref name="expansionFactor_"/>.</remarks>
        /// <param name="initialBuffer_">A span representing the initial buffer to be used for storing elements. The buffer must have sufficient
        /// capacity to hold the initial elements.</param>
        /// <param name="expansionFactor_">The factor by which the internal buffer will expand when additional capacity is required. Must be greater
        /// than or equal to 2. The default value is 2.</param>
        public xyList(Span<T> initialBuffer_, uint expansionFactor_ ): this((uint) expansionFactor_)
        {
            _viewPointerSpan = initialBuffer_;
        }
        /// <summary>
        /// Initialize a new instance of xyList with predefined capacity and expansion factor
        /// </summary>
        /// <param name="capacity_"></param>
        /// <param name="expansionFactor_"></param>
        public xyList(uint capacity_, uint expansionFactor_ = 2) : this((uint) expansionFactor_)
        {
            _rentedBufferArrayFromPool = ArrayPool<T>.Shared.Rent((int)capacity_);
            _viewPointerSpan = _rentedBufferArrayFromPool.AsSpan();
        }
        /// <summary>
        /// Initialize a new instance of xyList with expansion factor
        /// </summary>
        /// <param name="expansionFactor_"></param>
        public xyList(uint expansionFactor_)
        {
            _factor = expansionFactor_;
            _count = 0;
        }

        /// <summary>
        /// Add useful information
        /// </summary>
        public string Description { get; set; } = "My own little list like thingy";
        
        /// <summary>
        /// View/ Pointer
        /// Span represents a contiguous region of arbitrary memory... fancy shit
        /// It is type- and memory-SAFE. 
        /// Unlike arrays, it can point to either MANAGED or NATIVE memory, or to memory allocated on the STACK. 
        /// </summary>
        public Span<T> _viewPointerSpan;

        /// <summary>
        /// Sitting on the heap
        /// Rented from the Array-Pool
        /// Ideal for JSON-Parsing, Network Communication or editing pictures
        /// </summary>
        public T[]? _rentedBufferArrayFromPool;

        /// <summary>
        /// How many elements are in the span
        /// </summary>
        private int _count;

        /// <summary>
        /// Gets the number of elements currently contained in the collection.
        /// </summary>
        public int Count => _count;

        /// <summary>
        /// By how much do you want to multiply the size of reserved space in case of a resize
        /// </summary>
        private uint _factor;
        /// <summary>
        /// Gets the total number of elements that the underlying span can contain at MAX.
        /// </summary>
        public int Capacity => _viewPointerSpan.Length;

        /// <summary>
        /// Adds the specified item to the collection.
        /// </summary>
        /// <remarks>
        /// If the collection's capacity is exceeded, it will attempt to expand 
        /// --> Expand() to accommodate the new item.
        /// </remarks>
        /// <param name="target">The item to add to the collection. Cannot be null.</param>
        public void Add(T target)
        {
            if (_count >= _viewPointerSpan.Length)
            {
                Expand();

            }
            _viewPointerSpan[_count++] = target;
        }

        // TODO: Remove(target) 
        public void Remove(T target)
        {
            if (false)
            {

            }
        }


        /// <summary>
        /// Removes the element at the specified index from the collection.
        /// </summary>
        /// <remarks>After the element is removed, all subsequent elements are shifted one position to the
        /// left, and the size of the collection is reduced by one.</remarks>
        /// <param name="index">The zero-based index of the element to remove. Must be within the range of the collection.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="index"/> is less than 0 or greater than or equal to the number of elements in the
        /// collection.</exception>
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= _count)
            {
                throw new ArgumentOutOfRangeException(paramName: nameof(index));
            }
            else
            {
                // Select the targeted part of the span
                Span<T> tail = _viewPointerSpan.Slice(start: index + 1, length: _count - index - 1);

                // Move over
                tail.CopyTo(_viewPointerSpan.Slice(start: index));

                // Count down the number of elements
                _count--;

                // Delete reference!!!
                _viewPointerSpan[_count] = default!;

            }
        }

        /// <summary>
        /// Removes all elements from the collection.
        /// </summary>
        /// <remarks>After calling this method, the collection will be empty, and the count of elements will be
        /// reset to zero.</remarks>
        public void RemoveAll()
        {
            // Select all elements and clear them out
            _viewPointerSpan.Slice(start: 0, length: _count).Clear();

            // Set the element count to zero
            _count = 0;
        }




  


        /// <summary>
        /// Determines whether the current span is empty.
        /// </summary>
        /// <returns><see langword="true"/> if the span is empty; otherwise, <see langword="false"/>.</returns>
        public bool IsEmpty()
        {
            return _viewPointerSpan.IsEmpty;
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
                    return _viewPointerSpan[index];
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
                    _viewPointerSpan[index] = value;
                }
            }
        }


        /// <summary>
        /// Releases the rented array back to the shared array pool and clears its contents.
        /// </summary>
        /// <remarks>This method returns the rented array to the <see cref="ArrayPool{T}"/> and clears its
        /// contents  to ensure sensitive data is not retained in memory. After calling this method, the internal 
        /// buffer reference is set to its default value.</remarks>
        public void Dispose()
        {
            // if there is an array that is rented from the pool
            if (_rentedBufferArrayFromPool != null)
            {
                // Give the array back to the pool
                ArrayPool<T>.Shared.Return(array: _rentedBufferArrayFromPool, clearArray: true);

                // Delete the data 
                _viewPointerSpan = default!;

                // Delete the reference
                _rentedBufferArrayFromPool = default!;

            }
        }

        /// <summary>
        /// Expands the internal storage to accommodate additional elements.
        /// </summary>
        /// <remarks>This method increases the size of the internal buffer by allocating a larger array
        /// from the shared array pool. The contents of the current buffer are copied to the new array, and the old
        /// buffer is returned to the pool. The new size is determined by multiplying the current size by a growth
        /// factor or using a minimum size of 4, whichever is larger.</remarks>
        public void Expand()
        {
            // Set the bigger of the two values as new size for the array
            int newSize = Math.Max(val1: 4, val2: _viewPointerSpan.Length * (int)_factor);

            // Rent a new & bigger array from the pool
            T[] newPooledArray = ArrayPool<T>.Shared.Rent(minimumLength: newSize);

            // Select the specified part of the span and copy it into the newly rented array
            _viewPointerSpan.Slice(start: 0, length: _count).CopyTo(destination: newPooledArray);

            // if there is already an array, return it into the pool and delete the data
            if (_rentedBufferArrayFromPool != null)
            {
                ArrayPool<T>.Shared.Return(array: _rentedBufferArrayFromPool, clearArray: true);
            }

            // Copy the data
            _rentedBufferArrayFromPool = newPooledArray;

            // Set the reference
            _viewPointerSpan = newPooledArray;

        }


   

        /// <summary>
        /// Copies the elements of the current span into a new array.
        /// </summary>
        /// <remarks>The returned array is a new instance, and modifications to it will not affect the
        /// original span.</remarks>
        /// <returns>An array containing all the elements of the span. The length of the array matches the number of elements in
        /// the span.</returns>
        public T[] ToArray()
        {
            // Create a new array with the same length as the span
            T[]? result = new T[_count];

            // Select all items from the span and copy them into the new array 
            _viewPointerSpan.Slice(0, _count).CopyTo(result);

            // Show result
            return result;
        }
    }






    // Stack: volatile, extremely fast
    // Heap: Objects    (Garbage Collector works here)
    // Native/ Unmanaged: Ahuhu & Awawa... very unsafe





}
