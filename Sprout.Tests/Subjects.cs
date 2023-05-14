namespace Sprout.Tests;

public class Subjects {
    [Fact]
    public void TestSubjects() {
        SPObject testObject = SP.FileHandle("/Users/nebuladev/Documents/sprout/Subjects/Test.sp");
        
        Assert.Equal(testObject["array"]?[0], null);
        Assert.Equal(testObject["array"]?[1]["123"], 456);
        Assert.Equal(testObject["array"]?[2] + 1, 2);
    }
}
