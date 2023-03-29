namespace test;

public class Invalid
{
    /// <summary>
    /// Test that no exception is thrown when an unknown flag is encountered.
    /// And that valid flags are still processed.
    /// </summary>
    /// <param name="cmdline"> the command line to parse </param>
    /// <param name="expected"> the expected number of valid flags </param>
    [Theory]
    [InlineData("a a c d", 3)] // 1 unknown flag
    [InlineData("a c d e", 2)] // 2 unknown flags 
    public void WithoutDefaultHandler(string cmdline, int expected)
    {
        var cmd = new Parser(cmdline);

        int actual = 0;

        cmd.AddHandler(new Flag("a", "A"), (context) => { actual++; });
        cmd.AddHandler(new Flag("b", "B"), (context) => { actual++; });
        cmd.AddHandler(new Flag("c", "C"), (context) => { actual++; });
        
        cmd.Process();

        Assert.Equal(expected, actual);
    }
}