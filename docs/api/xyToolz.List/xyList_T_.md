# struct xyList<T>

Namespace: `xyToolz.List`  
Visibility: `public ref`  
Source: `xyToolz\List\xyList.cs`

## Description:

/// Sits on the stack and can NOT go on the heap!
    /// This can neither be used in async/await, iterators, interfaces nor in fields 
    ///

## Konstruktoren

- `xyList(Span<T> initialBuffer_, uint expansionFactor_ )` — `public`
  
  /// Initializes a new instance of the  class with the specified initial buffer and
        /// expansion factor.
        ///
- `xyList(uint capacity_, uint expansionFactor_ = 2)` — `public`
  
  /// Initialize a new instance of xyList with predefined capacity and expansion factor
        ///
- `xyList(uint expansionFactor_)` — `public`
  
  /// Initialize a new instance of xyList with expansion factor
        ///

## Eigenschaften

- `int Capacity` — `public`
  
  /// Gets the total number of elements that the underlying span can contain at MAX.
        ///
- `int Count` — `public`
  
  /// Gets the number of elements currently contained in the collection.
        ///
- `string Description{ get; set; }` — `public`
  
  /// Add useful information
        ///

## Methoden

- `bool IsEmpty()` — `public`
  
  /// Determines whether the current span is empty.
        ///
- `T[] ToArray()` — `public`
  
  /// Copies the elements of the current span into a new array.
        ///
- `void Add(T target)` — `public`
  
  /// Adds the specified item to the collection.
        ///
- `void Dispose()` — `public`
  
  /// Releases the rented array back to the shared array pool and clears its contents.
        ///
- `void Expand()` — `public`
  
  /// Expands the internal storage to accommodate additional elements.
        ///
- `void Remove(T target)` — `public`
  
  (No XML‑Summary )
- `void RemoveAll()` — `public`
  
  /// Removes all elements from the collection.
        ///
- `void RemoveAt(int index)` — `public`
  
  /// Removes the element at the specified index from the collection.
        ///

## Felder

- `Span<T> _viewPointerSpan` — `public`
  
  /// View/ Pointer
        /// Span represents a contiguous region of arbitrary memory... fancy shit
        /// It is type- and memory-SAFE. 
        /// Unlike arrays, it can point to either MANAGED or NATIVE memory, or to memory allocated on the STACK. 
        ///
- `T[]? _rentedBufferArrayFromPool` — `public`
  
  /// Sitting on the heap
        /// Rented from the Array-Pool
        /// Ideal for JSON-Parsing, Network Communication or editing pictures
        ///

