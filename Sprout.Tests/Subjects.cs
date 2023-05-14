namespace Sprout.Tests;

public class Subjects {
    [Fact]
    public void TestSubjects() {
        SPObject testObject = SP.FileHandle("/Users/nebuladev/Documents/sprout/Subjects/Test.sp");
        string shouldObject = "{\\n    123 = 456;\\n    abc = def;\\n}";
        Assert.Equal(testObject["array"]?[0], 1);
        Assert.Equal(testObject["array"]?[1].ToString().Replace("\n", "\\n").Replace("\t", "\\t"), shouldObject);
        Assert.Equal(testObject["array"]?[1]["123"], 456);
        Assert.Equal(testObject["array"]?[2] + 1, 4);
    }
}