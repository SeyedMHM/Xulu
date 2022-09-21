using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xulu.Controllers;
using Xulu.Services;

namespace Xulu.Test.Controllers
{
    public class CalculatorsControllerTest
    {
        private readonly Mock<ICalculatorService> _calculatorService = new Mock<ICalculatorService>();
        private readonly CalculatorsController _calculatorsController;
        public CalculatorsControllerTest()
        {
            _calculatorService
                .Setup(q => q.Calculate("abcd abcd aabbc ab a c ccd dede cccd cd"))
                .Returns(861);

            _calculatorService
               .Setup(q => q.Calculate("aabbcccca"))
               .Returns(25);

            _calculatorService
               .Setup(q => q.Calculate("abab ab de ee"))
               .Returns(31);

            _calculatorService
               .Setup(q => q.Calculate("abab ab de ee"))
               .Returns(31);

            _calculatorService
               .Setup(q => q.Calculate("dede abcd abd abddd ddada dac abcd de ede"))
               .Returns(2656);

            _calculatorService
               .Setup(q => q.Calculate("abcd a b dede bcde c d ee"))
               .Returns(-35);

            _calculatorService
               .Setup(q => q.Calculate("abcd a b c d"))
               .Returns(30);
           
            _calculatorsController = new CalculatorsController(_calculatorService.Object);
        }


        [Theory]
        [InlineData("abc")]
        [InlineData("aaa")]
        [InlineData("cbaA")]
        public void When_Input_Is_Valid_Should_Return_Sum(string operatoins)
        {
            IActionResult result = _calculatorsController.Execute(operatoins);
            var okObjectResult = result as OkObjectResult;

            Assert.NotNull(okObjectResult);
            Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
        }


        [Theory]
        [InlineData("abc")]
        [InlineData("aaa")]
        [InlineData("cbaA")]
        public void When_Input_Is_Valid_Should_Return_Status_200(string operatoins)
        {
            IActionResult result = _calculatorsController.Execute(operatoins);
            var okObjectResult = result as OkObjectResult;

            Assert.NotNull(okObjectResult);
            Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
        }


        [Theory]
        [InlineData("abcd abcd aabbc ab a c ccd dede cccd cd", 861)]
        [InlineData("aabbcccca", 25)]
        [InlineData("abab ab de ee", 31)]
        [InlineData("dede abcd abd abddd ddada dac abcd de ede", 2656)]
        [InlineData("abcd a b dede bcde c d ee", -35)]
        [InlineData("abcd a b c d", 30)]
        public void When_Input_Is_Valid_Should_Return_Sum_Of_Operations(string operatoins, double excepted)
        {
            IActionResult result = _calculatorsController.Execute(operatoins);

            var okObjectResult = result as OkObjectResult;

            Assert.NotNull(okObjectResult);
            Assert.Equal(excepted, okObjectResult.Value);
        }

        
        [Fact]
        public void When_Call_Execute_Action_Should_Call_Exactly_One_Calculate_Method_In_CalclulatorService()
        {
            _calculatorsController.Execute(It.IsAny<string>());

            _calculatorService.Verify(q => q.Calculate(It.IsAny<string>()), Times.Once);
        }
    }
}