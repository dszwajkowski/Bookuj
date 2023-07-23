using AutoMapper;
using Booking.Application.Common.Exceptions;
using Booking.Application.Common.Interfaces;
using Booking.Application.Common.Models;
using Booking.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Booking.Application.Features.Commands.Users
{
    public class UpdateUserCommand : IRequest
    {
        public string UserID { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string AddressLine { get; set; }
        public int? CityID { get; set; }
        public string PostalCode { get; set; }
        public string Description { get; set; }
        public IFormFile Avatar { get; set; }
    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly IApplicationDataContext _context;
        private readonly ICurrentUserService _currentUser;
        private readonly IFileManager _fileManager;
        private readonly IMapper _mapper;

        public UpdateUserCommandHandler(IApplicationDataContext context, IFileManager fileManager, IMapper mapper, ICurrentUserService currentUser)
        {
            _context = context;
            _fileManager = fileManager;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            if (request.UserID != _currentUser.ID)
            {
                throw new NotAuthorizedException();
            }
                            
            var user = await _context.Users
                .Include(u => u.Avatar)
                .Where(u => u.Id == request.UserID)
                .SingleAsync(cancellationToken);

            user.Email = request.Email;
            user.NormalizedEmail = request.Email.ToUpper();
            user.FirstName = request.FirstName;
            user.SecondName = request.SecondName;
            user.PhoneNumber = request.PhoneNumber;
            user.AddressLine = request.AddressLine;
            user.PostalCode = request.PostalCode;
            user.CityID = request.CityID;
            user.Description = request.Description;

            if (request.Avatar is not null)
            {
                if (user.Avatar.ID != 1 || !user.Avatar.FileName.StartsWith("default."))
                {
                    _fileManager.Delete(user.Avatar.FullPath);
                }
                FileModel file = await _fileManager.Upload(request.Avatar, $@"images\profiles\{user.Id}");
                user.Avatar = _mapper.Map<UserAvatar>(file);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
