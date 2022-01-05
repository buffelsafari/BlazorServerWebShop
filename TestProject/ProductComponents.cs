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



    
    }


}