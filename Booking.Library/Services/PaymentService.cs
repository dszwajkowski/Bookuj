using Booking.Application.Common.Interfaces;
using Booking.Application.Dtos;

namespace Booking.Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        public async Task<string> CreateOrderPayment(OrderDto order)
        {
            throw new NotImplementedException();
            var baseAddress = new Uri("https://private-anon-9f8dc291db-payu21.apiary-mock.com/");

            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("authorization", "Bearer 3e5cac39-7e38-4139-8fd6-30adc06a61bd");

                using (var content = new StringContent("{  \"notifyUrl\": \"https://your.eshop.com/notify\",  \"customerIp\": \"127.0.0.1\",  \"merchantPosId\": \"145227\",  \"description\": \"RTV market\",  \"currencyCode\": \"PLN\",  \"totalAmount\": \"21000\",  \"products\": [    {      \"name\": \"Wireless mouse\",      \"unitPrice\": \"15000\",      \"quantity\": \"1\"    },    {      \"name\": \"HDMI cable\",      \"unitPrice\": \"6000\",      \"quantity\": \"1\"    }  ]}", System.Text.Encoding.Default, "application/json"))
                {
                    using (var response = await httpClient.PostAsync("api/v2_1/orders/", content))
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                    }
                }
            }

        }
    }
}
