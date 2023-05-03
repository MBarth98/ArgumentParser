namespace test.types;

public class Boolean
{
    [Theory]
    [InlineData("test=True",  true)]
    [InlineData("test=False", false)]
    [InlineData("test=TRUE",  true)]
    [InlineData("test=FALSE", false)]
    [InlineData("test=true",  true)]
    [InlineData("test=false", false)]
    [InlineData("test=TrUe",  true)]  // by induction we can assume that other cases are handled
    [InlineData("test=faLsE", false)] // by induction we can assume that other cases are handled
    public void Traditional(string cmdline, bool expected)
    {

    }

    #if EXPERIMENTAL

    [Theory]
    [InlineData("test=0", false)]
    [InlineData("test=1", true)]
    [InlineData("test=2", true)] // by induction we can assume that all Natural+ numbers are true
    // language specific data types
    [InlineData("test=2147483647", true)] // int.MAX
    [InlineData("test=2147483648", true)] // int.MAX + 1
    [InlineData("test=4294967295", true)] // uint.MAX
    [InlineData("test=4294967296", true)] // uint.MAX + 1
    [InlineData("test=9223372036854775807",  true)] // long.MAX
    [InlineData("test=9223372036854775808",  true)] // long.MAX + 1
    [InlineData("test=18446744073709551615", true)] // ulong.MAX
    [InlineData("test=18446744073709551616", true)] // ulong.MAX + 1    
    public void Numbers(string cmdline, bool expected)
    {

    }

    #endif

    #if EXPERIMENTAL

    [Theory]
    [InlineData("test=off", false)]
    [InlineData("test=on",  true)]
    [InlineData("test=OFF", false)]
    [InlineData("test=ON",  true)]
    [InlineData("test=Off", false)]
    [InlineData("test=On",  true)]
    [InlineData("test=ofF", false)]
    [InlineData("test=oN",  true)]
    public void Switch(string cmdline, bool expected)
    {

    }

    #endif
}
