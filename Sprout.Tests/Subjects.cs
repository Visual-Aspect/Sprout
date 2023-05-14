namespace Sprout.Tests;

public class Subjects {
    [Fact]
    public void TestSubjects() {
        Console.WriteLine(SP.FileHandle("/Users/nebuladev/Documents/sprout/Subjects/Test.sp")["number"]);
    }
}