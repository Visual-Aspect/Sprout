using System.Diagnostics;

namespace Sprout.Tests;

public class Building {
    [Fact]
    public void Build() {
        SPObject testObject = new SPObject();
        testObject["name"] = "Test";
        testObject["int"] = 123;
        testObject["null"] = null;
        testObject["array"] = new SPArray();
        testObject["array"][0] = "Hello";
        testObject["array"][1] = new SPObject();
        testObject["array"][1]["World"] = "!";

        Console.WriteLine(testObject.ToString(0, 4));
    }
}
