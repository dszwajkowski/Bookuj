using AutoMapper;
using Booking.Application.Common.Models;
using Booking.Application.Dtos;
using Booking.Application.Features.Commands.Offers;
using Booking.Domain.Entities;
using Microsoft.AspNetCore.Identity;

#nullable disable

namespace Booking.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Offer, UpdateOfferCommand>()
                .ForMember(d => d.CityID, opt => opt.MapFrom(s => s.City.ID));
            CreateMap<Offer, OfferDto>()
                .ForMember(d => d.CityID, opt => opt.MapFrom(s => s.City.ID))
                .ForMember(d => d.CityName, opt => opt.MapFrom(s => s.City.Name))
                .ForMember(d => d.AuthorID, opt => opt.MapFrom(s => s.Author.Id))
                .ForMember(d => d.Username, opt => opt.MapFrom(s => s.Author.UserName))
                .ForMember(d => d.Email, opt => opt.MapFrom(s => s.Author.Email))
                .ForMember(d => d.Phone, opt => opt.MapFrom(s => s.Author.PhoneNumber))
                .ForMember(d => d.AvatarPath, opt => opt.MapFrom(s => s.Author.Avatar.FullPath));
                //.ForMember(d => d.LodgingOptions, opt => opt.MapFrom(s => s.LodgingOptions));
            CreateMap<Offer, OfferBriefDto>()
                .ForMember(d => d.CityID, opt => opt.MapFrom(s => s.City.ID))
                .ForMember(d => d.CityName, opt => opt.MapFrom(s => s.City.Name))
                .ForMember(d => d.AuthorId, opt => opt.MapFrom(s => s.Author.Id))
                .ForMember(d => d.Username, opt => opt.MapFrom(s => s.Author.Email));
            CreateMap<OfferFromStoredProcedure, OfferBriefDto>()
                .ForMember(d => d.ThumbnailPath, opt => opt.MapFrom(s => s.FullPath));
                //.AfterMap((src, dest) =>
                //{
                //    dest.OfferPhotos = new List<OfferPhotoDto>()
                //    {
                //        new OfferPhotoDto() { Path = src.Path, FileName = src.FileName }
                //    };
                //});
            CreateMap<LodgingOption, LodgingOptionDto>()
                .ForMember(d => d.LodgingFacilities, opt => opt.MapFrom(s => s.LodgingFacilities));
            CreateMap<LodgingOptionDto, LodgingOption>()
                .ForMember(d => d.LodgingFacilities, opt => opt.MapFrom(s => s.LodgingFacilities));
            CreateMap<LodgingFacilities, LodgingFacilitiesDto>();
            CreateMap<Reservation, ReservationDto>()
                .ForMember(d => d.Offer, opt => opt.MapFrom(s => s.LodgingOption.Offer));
            CreateMap<ReservationStatus, ReservationStatusDto>();
            CreateMap<Cart, CartDto>();
            CreateMap<FileModel, OfferPhoto>()
                .ForMember(d => d.Path, opt => opt.MapFrom(s => s.Url))
                .ForMember(d => d.FileName, opt => opt.MapFrom(s => s.FileName));
            CreateMap<FileModel, UserAvatar>()
                .ForMember(d => d.Path, opt => opt.MapFrom(s => s.Url))
                .ForMember(d => d.FileName, opt => opt.MapFrom(s => s.FileName));
            CreateMap<OfferPhoto, OfferPhotoDto>();
            CreateMap<User, UserBriefDto>()
                .ForMember(d => d.AvatarPath, opt => opt.MapFrom(s => s.Avatar.FullPath));
            CreateMap<User, UserDto>()
                .ForMember(d => d.AvatarPath, opt => opt.MapFrom(s => s.Avatar.FullPath))
                .ForMember(d => d.Avatar, opt => opt.Ignore());
            CreateMap<OfferOpinion, OfferOpinionDto>()
                .ForMember(d => d.Username, opt => opt.MapFrom(s => s.User.UserName))
                .ForMember(d => d.AvatarPath, opt => opt.MapFrom(s => s.User.Avatar.FullPath));
            CreateMap<PaymentMethod, PaymentMethodDto>();
            CreateMap<Order, OrderDto>();
            CreateMap<IdentityRole, RoleDto>();
        }
    }
}
