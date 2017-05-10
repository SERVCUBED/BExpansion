# BExpansion
Another general .NET expansion library for stuff I find useful.

[![NuGet](https://img.shields.io/nuget/v/SERVCUBED.BExpansion.svg)](https://www.nuget.org/packages/SERVCUBED.BExpansion/)

# Reference
## BExpansion

### Util

#### static string SendWebRequest(string url, bool quick = false, string postData = null, string userAgent = null, string referrer = null)
Send a WebRequest (with a timeout of 1000ms). Returns an empty string on timeout.

#### static string ReadFile(string url, Encoding encoding, string onError = null)
Read a file without locking it.

#### static void WriteFileAsync(string uri, string contents, Encoding encoding)
Writes a file in another thread.

#### static void WriteFile(string url, string contents, Encoding encoding)
Writes a file to the specified path.

#### static string GenerateRandomString()
Generate a random string. Not cryptographically secure.

### SafeThreadManager(int waitInterval = 10, int maxThreadCount = 50)

#### void RunThread(Action func, Action<Exception> exceptionCallback = null, ThreadPriority threadPriority = ThreadPriority.Normal)
Performs an action in a threadsafe environment.

#### void WaitForFinish()
Pause the current thread while all other threads finish.

#### int RunningThreads
The number of currently running threads.

### Expand

#### static string Join\<T\>(this IEnumerable\<T\> iEnumerable, string separator)
Concatenates the members of a collection, using the specified separator between each member.
This is a wrapper for the String.Join function.

#### static string AddDirectorySeparatorChar(this string path)
Adds a directory separator char to the end of a path if there is not one already.

#### static string MakePathSafe(this string path, char replacementChar = '\0')
Returns the string with all invalid path characters filtered.

#### static string MakeFileNameSafe(this string path, char replacementChar = '\0')
Returns the string with all invalid filename characters filtered.

#### static int Next(this RandomNumberGenerator r, int min, int max)
Returns the next value from the System.Security.Cryptography.RandomNumberGenerator.

#### static int Next(this RandomNumberGenerator r, int max) => Next(r, 0, max);
Returns the next value from the System.Security.Cryptography.RandomNumberGenerator.

#### static string GenerateRandomString(this Random r)
Generate a random string. Not cryptographically secure.

#### static string GenerateRandomString(this RandomNumberGenerator r)
Generate a cryptographically secure random string.

## BExpansion.Collections

### LoopedList\<T\>
Represents a strongly typed list of objects that can be indexed by index. The list appears to repeat infinitely
(e.g. for a list of length 2, the zero-based index '2' will return the first value).

    var looped = new BExpansion.Collections.LoopedList<string>();

    looped.Add("test1");
    looped.Add("test2");
    looped.Add("test3");
    
    Assert.AreEqual("test1", looped[0]);
    Assert.AreEqual("test2", looped[1]);
    Assert.AreEqual("test3", looped[2]);
    
    Assert.AreEqual("test1", looped[3]);
    Assert.AreEqual("test2", looped[4]);
    Assert.AreEqual("test3", looped[5]);
