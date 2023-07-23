namespace Booking.Application.Dtos
{
    public class UserBriefDto
    {
        public string ID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public DateTime? LockUntil { get; set; }
        public string AvatarPath { get; set; }
        public ICollection<UserRoleDto> Roles { get; set; } = new List<UserRoleDto>();

        public bool IsBlocked => LockUntil is not null && LockUntil > DateTime.Now;
    }
}
