namespace test;

public class Order
{
    [Fact]
    public void Test()
    {
        var exec = new Executor();
        
        var order = new List<string>();
        exec.AddHandler(new Flag("a", "A"), (context) => { order.Add("a"); });
        exec.AddHandler(new Flag("b", "B"), (context) => { order.Add("b"); });
        exec.AddHandler(new Flag("c", "C"), (context) => { order.Add("c"); });
        exec.AddHandler(new Flag("d", "D"), (context) => { order.Add("d"); });

        new Scanner("a b c d", exec).ParseAndCommit();
        Assert.Equal(new List<string> { "a", "b", "c", "d" }, order);
    }

}
