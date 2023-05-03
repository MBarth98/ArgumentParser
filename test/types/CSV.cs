namespace test.types;

public class CSV
{
    [Theory]
    [InlineData("csv=123", 1)]
    [InlineData("csv=1,2,3", 3)]    
    // space allowed if current ends with comma or next starts with comma
    [InlineData("csv=1,2, 3", 3)] 
    [InlineData("csv=1, 2 ,3", 3)]
    // no space allowed (separate argument - default)
    [InlineData("csv=1,2 3", 2)] 
    public void ElementCount(string cmdline, int expected)
    {
        var exec = new Executor();
        exec.AddHandler(new Property("csv", "CommaSeparatedList", VALUE_TYPE_ENUM.CSV), (context) => {
            Assert.Equal(context.value.Split(',').Length, expected);
        });

        var cmd = new Scanner(cmdline, exec);


        cmd.CallHandlers();
    }
}
