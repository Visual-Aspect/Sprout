using System.Diagnostics;

namespace SproutNS.Tests;

public class Subjects {
    [Fact]
    public void TestObject() {
        SPObject testObject = Sprout.FileHandle("/Users/nebuladev/Documents/sprout/Subjects/Test.sp");
        
        Assert.Equal(testObject["array"][0], null);
        Assert.Equal(testObject["array"][1]["123"], 456);
        Assert.Equal(testObject["array"][2] + 1, 2);
    }

    [Fact]
    public void TestArray() {
        SPArray testArray = Sprout.FileHandle("/Users/nebuladev/Documents/sprout/Subjects/Array.sp");

        for (int i = 1; i <= testArray.Variables.Count(); i++) Assert.Equal(testArray[i - 1], i);
    }

    [Fact]
    public void TimeTest() {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        List<int> testList = new List<int>();
        Stopwatch currentStopwatch = new Stopwatch();

        for (int i = 0; i < 1000; i++) {
            currentStopwatch.Restart();
            Sprout.FileHandle("/Users/nebuladev/Documents/sprout/Subjects/Test.sp");
            currentStopwatch.Stop();
            testList.Add((int)currentStopwatch.ElapsedMilliseconds);
        }

        stopwatch.Stop();
        Console.WriteLine("File reading: " + stopwatch.ElapsedMilliseconds);
        Console.WriteLine("Average: " + testList.Average());

        string fileData = File.ReadAllText("/Users/nebuladev/Documents/sprout/Subjects/Test.sp");

        stopwatch.Restart();
        testList.Clear();

        for (int i = 0; i < 1000; i++) {
            currentStopwatch.Restart();
            Sprout.Parse(fileData);
            currentStopwatch.Stop();
            testList.Add((int)currentStopwatch.ElapsedMilliseconds);
        }

        stopwatch.Stop();
        Console.WriteLine("Parsing: " + stopwatch.ElapsedMilliseconds);
        Console.WriteLine("Average: " + testList.Average());
    }

    [Fact]
    public void Serialize() {
        SPObject testObject = Sprout.FileHandle("/Users/nebuladev/Documents/sprout/Subjects/Test.sp");
        
        string serialized = Sprout.ObjectToString(testObject);
        Console.WriteLine(serialized);
    }
}
