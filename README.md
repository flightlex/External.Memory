## C# library on `netstandard 2.0` (supports `.NET Framework 4.6.1 and higher`), that allows you to externally read and write memory into the process, if you have the required access.

### Initialization
When initializing process, you have 3 choices how to do so.

#### Using process ID
```csharp
using External.Memory;
using System; // for console

// imaginary process id
var myProcessId = 1234;
var processMemory = new ProcessMemory(myProcessId);

Console.WriteLine("Process has been accessed");
```

#### Using process name
```csharp
using External.Memory;
using System; // for console

// .exe IS REQUIRED
var myProcessName = "mygame.exe";
var processMemory = new ProcessMemory(myProcessName);

Console.WriteLine("Process has been accessed");
```

#### Or the very last option, if you already have the process handle with all permissions (Full Access Flag: 0x001F0FFF)
```csharp
using External.Memory;
using System; // for console

// imaginary process handle
var myProcessHandle = new IntPtr(0x1488);
var processMemory = new ProcessMemory(myProcessHandle);

Console.WriteLine("Process has been accessed");
```

### Reading process memory
When it comes to reading, you need only 2 things:
- Address in memory (and the offset if needed).
- Value type that you want to read.

Value type is important because it has to read right amount of bytes at that address.
In our test-case we will try to obtain imaginary health in the our imagined game.

```csharp
using External.Memory;
using System; // for console

// we will use process name in our test case
var processMemory = new ProcessMemory("mygame.exe")

// some imaginary health address
nint healthOffset = 0x00001337;

// reading the "health" of type uint (unsigned int, 4 bytes)
var health = processMemory.Read<uint>(healthOffset);

// then logging the health into the console
Console.WriteLine($"Health: {health}");
```

### Writing process memory
Writing memory is basically the same, but beside the address and value-type, you need the values itself obviously, so provide it in the second argument.
Now lets try to change the health in our game.

```csharp
using External.Memory;
using System; // for console

// we will use process name in our test case
var processMemory = new ProcessMemory("mygame.exe")

// some imaginary health address
nint healthOffset = 0x00001337;

// reading the "health" of type uint (unsigned int)
var health = processMemory.Read<uint>(healthOffset);

// then logging the health into the console
Console.WriteLine($"Health: {health}");

// now lets change the health
processMemory.Write<uint>(healthOffset, 100);

// displaying new health
health = processMemory.Read<uint>(healthOffset)
Console.WriteLine($"New health: {health}");
```

### Additions
If you need, you can read a byte array with your desired length, and then manipulate the buffer.

```csharp
using External.Memory;
using System; // for console

// we will use process name in our test case
var processMemory = new ProcessMemory("mygame.exe")

// some imaginary health address
nint healthOffset = 0x00001337;

// reading the "health" as byte array
var healthArray = processMemory.ReadBytes(healthOffset, 4);

// now we can manually transform our buffer to a uint value using BitConverter class
var health = BitConverter.ToUInt32(healthArray, 0); // 0 is a start index
```

The result should stay the same.

### Other features
- Getting `Module Handle` and/or `Module Base Address` in your process.

```csharp
using External.Memory;
using System; // for console

// we will use process name in our test case
var processMemory = new ProcessMemory("mygame.exe")

// imagined module
var moduleName = "client.dll";
var clientDllHandle = processMemory.GetModuleHandle(moduleName); // return type: IntPtr
var clientDllBaseAddress = processMemory.GetModuleBaseAddress(moduleName); // return type: IntPtr
```

- Getting `Proc Address` in your process _(Address of an exported function or variable from the specified dynamic-link-library (DLL))_

```csharp
using External.Memory;
using System; // for console

// we will use process name in our test case
var processMemory = new ProcessMemory("mygame.exe")

// imagined exported function
var functionName = "myExportedFunction";
var myExportedFunctionAddress = processMemory.GetProcAddress(functionName) // return type;

// or you can even get proc address of a different handle
// for example if you need an exported function from certain module that you might have obtained earlier
// you can provide it as function is overloaded

var anotherFunctionName = "myAnotherExportedFunction";
var myModuleHandle = new IntPtr(0x12345);
var myAnotherExportedFunction = processMemory.GetProcAddress(myModuleHandle, anotherFunctionName) // return type;
```

### TODO
- Add signature scans
- Add tests as i am a lazy ass
