using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xulu.Controllers;
using Xulu.Services;

namespace Xulu.Test.Services
{
    public class CalculatorServiceTest
    {
        private readonly CalculatorService _calculatorService;
        private readonly Mock<ICalculatorValidationService> _calculatorValidationService = new Mock<ICalculatorValidationService>();
        public CalculatorServiceTest()
        {
            _calculatorValidationService
                .Setup(q => q.IsValidOperations(null))
                .Throws(new ArgumentNullException());

            _calculatorValidationService
                .Setup(q => q.IsValidOperations(It.IsRegex(@"^[\s]{0,}$")))
                .Throws(new ArgumentNullException());

            _calculatorValidationService
                .Setup(q => q.IsValidOperations(It.IsRegex(@"[^a-eA-E\s]")))
                .Throws(new ArgumentException());

            _calculatorService = new CalculatorService(_calculatorValidationService.Object);
        }


        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void When_Input_Is_Null_Or_Empty_Should_Return_Exception(string operatoins)
        {
            Assert.Throws<ArgumentNullException>(() => _calculatorService.Calculate(operatoins));
        }


        [Theory]
        [InlineData("ab z")]
        [InlineData("1")]
        [InlineData("F0")]
        public void When_Input_Is_Not_Valid_Should_Return_Exception(string operatoins)
        {
            Assert.Throws<ArgumentException>(() => _calculatorService.Calculate(operatoins));
        }


        [Fact]
        public void When_Call_Execute_Action_Should_Call_Exactly_One_IsValidOperations_Method_In_CalculatorValidationService()
        {
            Assert.Throws<ArgumentNullException>(() => _calculatorService.Calculate(It.IsAny<string>()));

            _calculatorValidationService.Verify(q => q.IsValidOperations(It.IsAny<string>()), Times.Once);
        }

    }
}
