using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text.RegularExpressions;
using Xulu.Services;

namespace Xulu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculatorsController : ControllerBase
    {
        private readonly ICalculatorService _calculatorService;

        public CalculatorsController(ICalculatorService calculatorService)
        {
            _calculatorService = calculatorService;
        }

        [HttpPost]
        public IActionResult Execute(string operations)
        {
            if (!string.IsNullOrEmpty(operations))
            {
                operations = operations.Trim();
            }

            var result = _calculatorService.Calculate(operations);

            return Ok(result);
        }
    }
}
