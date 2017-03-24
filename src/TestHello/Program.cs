using System.IO;
using Fonet;

namespace FonetExample
{
    class HelloWorld
    {
        static void Main(string[] args)
        {
            FonetDriver driver = FonetDriver.Make();
            driver.ImageHandler += str =>
            {
                return null;
            };
            driver.Render("..\\..\\hello.fo", "hello.pdf");
        }
    }
}