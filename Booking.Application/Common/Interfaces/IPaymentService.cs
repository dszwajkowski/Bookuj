using Booking.Application.Dtos;

namespace Booking.Application.Common.Interfaces
{
    public interface IPaymentService
    {
        Task<string> CreateOrderPayment(OrderDto order);
    //        using System;
    //using System.Net.Http;

    //var baseAddress = new Uri("https://private-anon-9f8dc291db-payu21.apiary-mock.com/");

    //using (var httpClient = new HttpClient{ BaseAddress = baseAddress
    //})
    //{

    //    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("authorization", "Bearer 3e5cac39-7e38-4139-8fd6-30adc06a61bd");

    //    using (var response = await httpClient.GetAsync("api/v2_1/paymethods/"))
    //    {

    //        string responseData = await response.Content.ReadAsStringAsync();
    //    }
//}
    }
}
