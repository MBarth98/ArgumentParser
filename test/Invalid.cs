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
        var exec = new Executor();
        int actual = 0;
        exec.AddHandler(new Flag("a", "A"), (context) => { actual++; });
        exec.AddHandler(new Flag("b", "B"), (context) => { actual++; });
        exec.AddHandler(new Flag("c", "C"), (context) => { actual++; });
        new Scanner(cmdline, exec).ParseAndCommit();

        Assert.Equal(expected, actual);
    }
}