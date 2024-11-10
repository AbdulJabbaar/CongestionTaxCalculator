using CongestionTaxCalculator.API.Response;
using System.Text.Json;

namespace CongestionTaxCalculator.IntegrationTests.StepDefinitions
{
    [Binding]
    public class CongestionTaxStepDefinitions
    {
        private readonly CongestionTaxCalculatorWebAppFactory _factory;
        private readonly HttpClient _httpClient;

        private const string BaseAddress = "http://localhost/";
        private HttpResponseMessage? _response;

        private string _vehicleType;
        private string _city;
        private List<string> _dates;

        public CongestionTaxStepDefinitions()
        {
            _factory = new CongestionTaxCalculatorWebAppFactory();
            _httpClient = _factory.CreateDefaultClient(new Uri(BaseAddress));
        }

        [AfterScenario]
        public void AfterScenario()
        {
            _httpClient.Dispose();
            _factory.Dispose();
        }

        [Given(@"a ""([^""]*)"" vehichle passing through city ""([^""]*)"" toll stations on ""([^""]*)"" at following times:")]
        public void GivenAVehichlePassingThroughCityTollStationsOnAtFollowingTimes(string regular, string gothenburg, string p2, Table table)
        {
            _vehicleType = regular;
            _city = gothenburg;
            _dates = new List<string>();

            foreach (var item in table.Rows)
            {
                var time = item.Values.First().Trim().Split(':');
                _dates.Add($"{p2}T{time[0]}:{time[1]}:00.000Z");
            }
        }


        [When(@"i calculate the toll fee for the day")]
        public void WhenICalculateTheTollFeeForTheDay()
        {
            using (var content = new MultipartFormDataContent())
            {
                content.Add(new StringContent(_city), "CityName");
                content.Add(new StringContent(_vehicleType), "VehicleType");

                for (int i = 0; i < _dates.Count; i++)
                {
                    content.Add(new StringContent(_dates[i]), $"DatePassesToll[{i}]");
                }

                _response = _httpClient.PostAsync(" city", content).Result;
            }
        }

        [Then(@"the total toll fee should be ""([^""]*)""")]
        public async Task ThenTheTotalTollFeeShouldBe(string tollFee)
        {
            var content = await _response.Content.ReadAsStringAsync();
            var taxResponse = JsonSerializer.Deserialize<VehicleTaxResponse>(content,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            
            taxResponse.TaxAmount.Should().Be(Convert.ToInt32(tollFee));
        }
    }
}
