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
        var cmd = new Parser(cmdline);
        
        cmd.AddHandler(new Property("enum", typeof(TestEnum)), (context) => {});

        if (willThrow)
        {
            Assert.ThrowsAny<Exception>(() => cmd.Process());
        }
        else
        {
            cmd.Process();
        }
    }
}
