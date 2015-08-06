using System;
using System.Linq.Expressions;
using AutoMapper;
using Exerp.Api.DataTransfer;
using PureRide.Web.ViewModels.Account;
using PureRide.Web.ViewModels.Booking;
using PureRide.Web.ViewModels.Credits;
using PureRide.Web.ViewModels.Location;

namespace PureRide.Web
{
    public static class MappingConfiguration
    {

        public static void ConfigureMappings()
        {
            //Mappings Between Source->Destination

            Mapper.CreateMap<RegisterPersonModel, PersonDetails>()
               .Ignore(a=>a.Address)
                .ForMember(a => a.Birthday, b => b.ResolveUsing(c => c.GetDateOfBirth()))
                .ForMember(a => a.WishToReceiveThirdPartyOffers, b => b.ResolveUsing(c => c.EmailOptIn))
                .ForMember(a => a.AllowSms, b => b.ResolveUsing(c => c.SmsOptIn)).Ignore(a=>a.Friends);

            Mapper.CreateMap<PersonDetails, UpdateDetailsModel>()
                .ForMember(a => a.DayOfBirth, b => b.ResolveUsing(c => c.Birthday.Day))
                .ForMember(a => a.MonthOfBirth, b => b.ResolveUsing(c => c.Birthday.Month))
                .ForMember(a => a.YearOfBirth, b => b.ResolveUsing(c => c.Birthday.Year));

            Mapper.CreateMap<UpdateDetailsModel, PersonDetails>()
                .ForMember(a => a.Birthday,
                    b => b.ResolveUsing(c => new DateTime(c.YearOfBirth, c.MonthOfBirth, c.DayOfBirth)))
                .Ignore(a => a.AllowSms)
                .Ignore(a => a.Address)
                .Ignore(a => a.WishToReceiveThirdPartyOffers)
                .Ignore(a => a.Friends)
                .Ignore(a => a.Password);

            Mapper.CreateMap<Centre, LocationModel>()
                .ForMember(a => a.Name, b => b.ResolveUsing(c => c.WebName));

            Mapper.CreateMap<Address, BillingAddressModel>()
                .ForMember(a => a.City, b => b.ResolveUsing(c => c.Address3))
                .ForMember(a => a.Country, b => b.ResolveUsing(c => "United Kingdom"))
                .Ignore(a => a.FirstNames)
                .Ignore(a => a.Surname);
            
             Mapper.CreateMap<BillingAddressModel,Address>()  
                .ForMember(a => a.Address3, b => b.ResolveUsing(c => c.City))
                .ForMember(a => a.Country, b => b.ResolveUsing(c => "GB"));

            Mapper.CreateMap<PersonFriend, FriendModel>().ForMember(a=>a.FriendId,b=>b.ResolveUsing(c=>c.FriendId.ToString()));
            Mapper.CreateMap<RegisterFriendModel, Person>();
            
        }

        /// <remarks>See "http://stackoverflow.com/a/16808867/33"</remarks>
        private static IMappingExpression<TSource, TDestination> Ignore<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> map,
            Expression<Func<TDestination, object>> selector)
        {
            map.ForMember(selector, config => config.Ignore());
            return map;
        }
    }
}
