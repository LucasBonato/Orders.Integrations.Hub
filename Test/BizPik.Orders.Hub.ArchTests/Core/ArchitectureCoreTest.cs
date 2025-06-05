using ArchUnitNET.Domain;
using ArchUnitNET.Loader;

namespace BizPik.Orders.Hub.ArchTests.Core;

public class ArchitectureCoreTest
{
    private static readonly Architecture Architecture = new ArchLoader()
        .LoadAssemblies(

        )
        .Build();

}