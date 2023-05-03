namespace test.types;

public class Enumeration
{
    public enum TestEnum
    {
        FIRST,
        SECOND,
    }

    [Theory]
    [InlineData("enum=FIRST", false)]
    [InlineData("enum=SECOND", false)]
    [InlineData("enum=THIRD", true)]
    public void Content(string cmdline, bool willThrow)
    {

    }
}
