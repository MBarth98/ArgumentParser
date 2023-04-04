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
        var exec = new Executor();
        exec.AddHandler(new Property("enum", typeof(TestEnum)), (context) => {});

        if (willThrow)
        {
            Assert.ThrowsAny<Exception>(() => new Scanner(cmdline, exec).ParseAndCommit());
        }
        else
        {
            new Scanner(cmdline, exec).ParseAndCommit();
        }
    }
}
