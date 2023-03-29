namespace test;

public class Order
{
    [Fact]
    public void Test()
    {
        var cmd = new Parser("a b c d");
        var order = new List<string>();
        cmd.AddHandler(new Flag("a", "A"), (context) => { order.Add("a"); });
        cmd.AddHandler(new Flag("b", "B"), (context) => { order.Add("b"); });
        cmd.AddHandler(new Flag("c", "C"), (context) => { order.Add("c"); });
        cmd.AddHandler(new Flag("d", "D"), (context) => { order.Add("d"); });
        cmd.Process();
        Assert.Equal(new List<string> { "a", "b", "c", "d" }, order);
    }

    [Fact]
    public void Test2()
    {
        
    }
}
