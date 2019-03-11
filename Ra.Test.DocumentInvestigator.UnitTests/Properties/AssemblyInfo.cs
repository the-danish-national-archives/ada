#region Namespace Using

using System.Reflection;
using System.Runtime.InteropServices;
using NUnit.Framework;

#endregion

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.

[assembly: AssemblyTitle("Ra.Test.DocumentInvestigator.UnitTests")]
[assembly: AssemblyDescription("")]
// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.

[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM

[assembly: Guid("fbe3057e-3843-4eee-b526-4238350f38b6")]


//[assembly: log4net.Config.XmlConfigurator(Watch = true)]

[assembly: Parallelizable(ParallelScope.Fixtures)]