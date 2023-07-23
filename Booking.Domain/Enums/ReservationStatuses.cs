namespace Booking.Domain.Enums
{
    /// <summary>
    /// Must represent statuses from database 
    /// </summary>
    public enum ReservationStatuses
    {
        NotConfirmed = 1,
        WaitingForConfirmation = 2,
        Confirmed = 3,
        Cancelled = 4,
    }
}
