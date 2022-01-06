using Xunit;
using Bunit;
using BlazorServerCrud1.Components.ProductComponents;


namespace Test.ProductComponents
{
    public class ProductComponents:TestContext
    {

        [Fact]
        public void Test1()
        { 
            var cut = RenderComponent<ProductTitle>(parameters=>parameters.AddChildContent("hello world"));
            cut.Find("div").MarkupMatches("<div>hello world</div>");
        }


        [Theory]
        [InlineData("hello world")]
        [InlineData("cruel world")]
        public void TestTheory1(string str)
        {
            var cut = RenderComponent<ProductTitle>(parameters => parameters.AddChildContent(str));
            cut.Find("div").MarkupMatches($"<div>{str}</div>");
        }


    }


}