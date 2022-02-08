
How to run guide:

- It's prefered to run it from VisualStudio with a play button, but if that's not feasable for you - see next point.

- Running from command line:
	With .NET you have several options of running projects. Reference: https://docs.microsoft.com/en-us/dotnet/core/deploying/
	The option we will use is "framework-dependent executable". This option assumes that you have .NET framework installed in your environment.
	.NET can be installed on Windows, MacOS and Linux with no issues. For supported versions on Linux, see: https://docs.microsoft.com/en-us/dotnet/core/install/linux#:~:text=NET%20is%20available%20on%20different,NET.&text=NET%20supports.

	1. Create executable - "dotnet publish [Solution]" - eg. "dotnet publish Minitwit_BE.sln"
	2. Run file - "dotnet [.dll file]" - eg. dotnet .\Minitwit_BE.Api\bin\Debug\net6.0\publish\Minitwit_BE.Api.dll


If you have any question visit the attached links or ask Szymon.