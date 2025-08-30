# class xyMessageFactory

Namespace: `xyToolz.Logging.Helper`  
Visibility: `public`  
Source: `xyToolz\Logging\Helper\xyMessageFactory.cs`

## Description:

/// Providing generalized messages for logging via a non static class 
    /// 
    /// The methods return strings for easy digestion
    /// 
    /// Really nice, lol
    ///

## Eigenschaften

- `string[]? Description{ get; set; }` — `public`
  
  /// Add usefull information
        ///

## Methoden

- `string ConfigurationError(string? config = null)` — `public`
  
  (No XML‑Summary )
- `string ConnectionStringNotFound(string? name = null)` — `public`
  
  /// No connection string found in config
        ///
- `string ContextNotSaved([MaybeNull] string? name = null)` — `public`
  
  /// Context didnt save
        ///
- `string ContextSaved(string name, int count)` — `public`
  
  /// Context saved [...] changes
        ///
- `string Created([MaybeNull] string? name = null)` — `public`
  
  /// Created target -> name
        ///
- `string Created([MaybeNull]int? ID = null)` — `public`
  
  /// Created target -> ID
        ///
- `string DatabaseConnectionFailed(string? dbName = null)` — `public`
  
  (No XML‑Summary )
- `string DatabaseQueryError(string? query = null)` — `public`
  
  (No XML‑Summary )
- `string DecryptionFailed(string? target = null)` — `public`
  
  (No XML‑Summary )
- `string Deleted([MaybeNull] int? ID = null)` — `public`
  
  /// Deleted target from DB
        ///
- `string DeserializationFail(string? name = null)` — `public`
  
  /// Failed to deserialize the target
        ///
- `string DeserializationSuccess(object? target = null)` — `public`
  
  /// Successfully deserialized the target
        ///
- `string EmptyList(string? nameOfTheList = null)` — `public`
  
  /// List is empty
        ///
- `string EncryptionFailed(string? target = null)` — `public`
  
  (No XML‑Summary )
- `string EntryAdded(string? contextName = null)` — `public`
  
  /// Entry added to context
        ///
- `string EntryCreated()` — `public`
  
  /// Created entry
        ///
- `string EntryCreatedAndAdded(string? contextName = null)` — `public`
  
  /// Entry created and added to context
        ///
- `string EntryEmptyList()` — `public`
  
  /// No entries in list
        ///
- `string EntryList()` — `public`
  
  /// List with context entries
        ///
- `string EntryNotCreated()` — `public`
  
  /// No entry created
        ///
- `string EntryNotFound(object ID)` — `public`
  
  /// No entry found
        ///
- `string EntryNotRemoved([MaybeNull] string? name = null)` — `public`
  
  /// Unable to remove entry from context
        ///
- `string EntryNotUpdated([MaybeNull] string? name = null)` — `public`
  
  /// Entry couldnt be updated
        ///
- `string EntryRemoved([MaybeNull]string? name = null)` — `public`
  
  /// Removed entry  from context
        ///
- `string EntryUpdated([MaybeNull] string? name = null)` — `public`
  
  /// Entry was updated
        ///
- `string FileAccessDenied(string? file = null)` — `public`
  
  (No XML‑Summary )
- `string FileNotFound(string? file = null)` — `public`
  
  (No XML‑Summary )
- `string FileReadError(string? file = null)` — `public`
  
  (No XML‑Summary )
- `string HostUnreachable(string? host = null)` — `public`
  
  (No XML‑Summary )
- `string InvalidCertificate(string? cert = null)` — `public`
  
  (No XML‑Summary )
- `string InvalidID(string ID)` — `public`
  
  /// Invalid ID
        ///
- `string KeyFound(string? key = null)` — `public`
  
  /// Key found
        ///
- `string KeyNotFound(string? key = null)` — `public`
  
  /// No key found
        ///
- `string KeyNotSet(string? key = null)` — `public`
  
  /// Failed to set the value of the target key to the target value
        ///
- `string KeySet(string? key = null)` — `public`
  
  /// Successfully managed to set the value of the target key
        ///
- `string LoginFail()` — `public`
  
  /// User failed to provide valid data for login
        ///
- `string LoginSuccess(string? username)` — `public`
  
  /// Login-data is valid
        ///
- `string ModelInvalid(string? modelName = null)` — `public`
  
  (No XML‑Summary )
- `string ModelSkipped(string? modelName = null)` — `public`
  
  (No XML‑Summary )
- `string ModelUnvalidated(string? modelName = null)` — `public`
  
  (No XML‑Summary )
- `string ModelValid(string? modelName = null)` — `public`
  
  (No XML‑Summary )
- `string NetworkUnavailable(string? host = null)` — `public`
  
  (No XML‑Summary )
- `string NotCreated()` — `public`
  
  /// Failed to create
        ///
- `string NotDeleted([MaybeNull] int? ID = null)` — `public`
  
  /// Failed to delete target
        ///
- `string NotRead(int? ID = null)` — `public`
  
  /// Failed to read the target data from DB
        ///
- `string OperationFailed(string? operation = null)` — `public`
  
  (No XML‑Summary )
- `string ParameterInvalid(string? paramName = null)` — `public`
  
  /// Parameter is incorrect
        ///
- `string ParameterIsNull(string? paramName = null)` — `public`
  
  /// Parameter is null
        ///
- `string ParameterValid(string? paramName = null)` — `public`
  
  /// Parameter is OK
        ///
- `string Read()` — `public`
  
  /// Read data from DB success
        ///
- `string SerializationFail(string? name = null)` — `public`
  
  /// Failed to serialize the target
        ///
- `string SerializationSuccess(object? target = null)` — `public`
  
  /// Successfully serialized the target
        ///
- `string TimeoutOccurred(string? operation = null)` — `public`
  
  (No XML‑Summary )
- `string TokenExpired(string? tokenId = null)` — `public`
  
  (No XML‑Summary )
- `string TokenGenerated(string? token = null)` — `public`
  
  /// Token generated 
        ///
- `string TokenNotGenerated()` — `public`
  
  /// Token generation failed
        ///
- `string UnknownError(string? details = null)` — `public`
  
  (No XML‑Summary )
- `string Updated()` — `public`
  
  /// Failed to update
        ///
- `string Updated([MaybeNull] int? ID = null)` — `public`
  
  /// Updated target -> ID
        ///
- `string Updated([MaybeNull] string? name = null)` — `public`
  
  /// Updated target -> name
        ///
- `string UserAlreadyExists(string? user = null)` — `public`
  
  (No XML‑Summary )
- `string UserLockedOut(string? user = null)` — `public`
  
  (No XML‑Summary )
- `string UserNotFound(string? user = null)` — `public`
  
  (No XML‑Summary )
- `string WrongPassword()` — `public`
  
  /// Password is wrong
        ///
- `string WrongUserName()` — `public`
  
  /// Username is wrong
        ///
- `string WrongUserNameOrPassword()` — `public`
  
  /// Username or password is wrong
        ///

