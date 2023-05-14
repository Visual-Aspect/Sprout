using System.Diagnostics;

namespace Sprout.Tests;

public class Subjects {
    [Fact]
    public void TestObject() {
        SPObject testObject = SP.FileHandle("/Users/nebuladev/Documents/sprout/Subjects/Test.sp");
        
        Assert.Equal(testObject["array"][0], null);
        Assert.Equal(testObject["array"][1]["123"], 456);
        Assert.Equal(testObject["array"][2] + 1, 2);
    }

    [Fact]
    public void TestArray() {
        SPArray testArray = SP.FileHandle("/Users/nebuladev/Documents/sprout/Subjects/Array.sp");

        for (int i = 1; i <= testArray.Variables.Count(); i++) Assert.Equal(testArray[i - 1], i);
    }

    [Fact]
    public void TimeTest() {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        for (int i = 0; i < 1000; i++) {
            SP.FileHandle("/Users/nebuladev/Documents/sprout/Subjects/Test.sp");
        }

        stopwatch.Stop();
        Console.WriteLine("File reading: " + stopwatch.ElapsedMilliseconds);

        string fileData = File.ReadAllText("/Users/nebuladev/Documents/sprout/Subjects/Test.sp");

        stopwatch.Restart();

        for (int i = 0; i < 1000; i++) {
            SP.Parse(fileData);
        }

        stopwatch.Stop();
        Console.WriteLine("Parsing: " + stopwatch.ElapsedMilliseconds);
    }
}
