namespace Booking.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        string ID { get; }
        //Task<bool> IsInRole(string roleName);
    }
}
