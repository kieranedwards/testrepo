using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Exerp.Api.DataTransfer;
using Exerp.Api.Helpers;
using Exerp.Api.WebServices.BookingAPI;
using Exerp.Api.WebServices.PersonAPI;
using Exerp.Api.WebServices.SubscriptionAPI;
using paymentMethod = Exerp.Api.WebServices.SubscriptionAPI.paymentMethod;


namespace Exerp.Api
{
    public static class MappingConfiguration
    {
        /// <summary>
        /// AutoMapper Mappings Between Source->Destination
        /// </summary>
        public static void ConfigureMappings()
        {
            Mapper.CreateMap<address, Address>().ForMember(a => a.PostCode, b => b.ResolveUsing(c => c.zip)); 
            Mapper.CreateMap<Address, address>().Ignore(a => a.coName).Ignore(a => a.zipName).ForMember(a=>a.zip,b=>b.ResolveUsing(c=>c.PostCode));

            Mapper.CreateMap<centerDetail, Centre>()
                .Ignore(a => a.Region)
                .ForMember(a => a.Postcode, b => b.ResolveUsing(c => c.address.zip))
                .ForMember(a => a.AddressLine1, b => b.ResolveUsing(c => c.address.address1))
                .ForMember(a => a.AddressLine2, b => b.ResolveUsing(c => c.address.address2))
                .ForMember(a => a.AddressLine3, b => b.ResolveUsing(c => c.address.address3))
                .ForMember(a => a.Longitude, b => b.ResolveUsing(c => c.coordinate.longitude))
                .ForMember(a => a.Latitude, b => b.ResolveUsing(c => c.coordinate.latitude));

            Mapper.CreateMap<person, PersonDetails>()
                .ForMember(a => a.IsMale, b => b.ResolveUsing(c => c.male))
                .ForMember(a => a.Address, b => b.ResolveUsing(c => Mapper.Map<Address>(c.address)))
                .Ignore(a => a.MobilePhoneNumber)
                .Ignore(a => a.Email)
                .Ignore(a => a.Password)
                .Ignore(a => a.AllowSms)
                .Ignore(a => a.WishToReceiveThirdPartyOffers)
                .Ignore(a => a.ShoeSize)
                .Ignore(a => a.Friends);

            Mapper.CreateMap<personDetail, PersonDetails>()
                .ForMember(a => a.IsMale, b => b.ResolveUsing(c => c.person.male))
                .ForMember(a => a.Address, b => b.ResolveUsing(c => Mapper.Map<Address>(c.person.address)))
                .ForMember(a => a.Email, b => b.ResolveUsing(c => c.personCommunication.email))
                .ForMember(a => a.FirstName, b => b.ResolveUsing(c =>c.person.firstName))
                .ForMember(a => a.LastName, b => b.ResolveUsing(c => c.person.lastName))
                .ForMember(a => a.Birthday, b => b.ResolveUsing(c => c.person.birthday.FromApiDateString()))
                .ForMember(a => a.ShoeSize, b => b.ResolveUsing(c =>
                {
                    var firstOrDefault = c.extendedAttributes.FirstOrDefault(a=>a.id==Constants.ShoeSizeEaKey);
                    return firstOrDefault != null ? firstOrDefault.value : null;
                }))
                .ForMember(a=>a.Friends,b=>b.ResolveUsing(c=>c.friends.Select(Mapper.Map<PersonFriend>)))
                .ForMember(a => a.MobilePhoneNumber, b => b.ResolveUsing(c => c.personCommunication.mobilePhoneNumber))
                .Ignore(a=>a.Password)
                .Ignore(a=>a.WishToReceiveThirdPartyOffers)
                .Ignore(a=>a.AllowSms);
              
            Mapper.CreateMap<WebServices.SubscriptionAPI.compositeKey, personKey>().Ignore(a=>a.externalId);
            Mapper.CreateMap<WebServices.SelfServiceAPI.compositeKey, personKey>().Ignore(a => a.externalId);
            Mapper.CreateMap<WebServices.PersonAPI.compositeKey, personKey>().Ignore(a => a.externalId);
            Mapper.CreateMap<WebServices.BookingAPI.compositeKey, personKey>().Ignore(a => a.externalId);

            Mapper.CreateMap<personKey, WebServices.SubscriptionAPI.compositeKey>();
            Mapper.CreateMap<personKey, WebServices.SelfServiceAPI.compositeKey>();
            Mapper.CreateMap<personKey, WebServices.PersonAPI.compositeKey>();
            Mapper.CreateMap<personKey, WebServices.BookingAPI.compositeKey>();

            Mapper.CreateMap<WebServices.SelfServiceAPI.compositeKey, Identity>().ConstructUsing(
                a => new Identity(a.center, a.id)); 
            
            Mapper.CreateMap<Identity,WebServices.PersonAPI.compositeKey>()
                .ConstructUsing(a => new WebServices.PersonAPI.compositeKey(){center=a.CentreId, id=a.EntityId})
                .Ignore(a=>a.center)
                .Ignore(a => a.id); 
            
            Mapper.CreateMap<WebServices.SubscriptionAPI.compositeKey, Identity>().ConstructUsing(
                a => new Identity(a.center, a.id));

            Mapper.CreateMap<WebServices.BookingAPI.compositeKey, Identity>().ConstructUsing(
               a => new Identity(a.center, a.id));
           
            Mapper.CreateMap<Identity, WebServices.SubscriptionAPI.compositeKey>()
                .ForMember(a => a.id, b => b.ResolveUsing(c => c.EntityId))
                .ForMember(a => a.center, b => b.ResolveUsing(c => c.CentreId));

            Mapper.CreateMap<Identity, WebServices.SelfServiceAPI.compositeKey>()
                 .ForMember(a => a.id, b => b.ResolveUsing(c => c.EntityId))
                 .ForMember(a => a.center, b => b.ResolveUsing(c => c.CentreId));

            Mapper.CreateMap<Identity, personKey>()
              .ForMember(a => a.id, b => b.ResolveUsing(c => c.EntityId))
              .ForMember(a => a.center, b => b.ResolveUsing(c => c.CentreId))
              .Ignore(a => a.externalId);

            Mapper.CreateMap<Identity, WebServices.BookingAPI.compositeKey>()
              .ForMember(a => a.id, b => b.ResolveUsing(c => c.EntityId))
              .ForMember(a => a.center, b => b.ResolveUsing(c => c.CentreId));

            Mapper.CreateMap<Identity, WebServices.SocialAPI.compositeKey>()
              .ForMember(a => a.id, b => b.ResolveUsing(c => c.EntityId))
              .ForMember(a => a.center, b => b.ResolveUsing(c => c.CentreId));

            Mapper.CreateMap<seat, Seat>()
                .ForMember(a => a.CoordinateX, b => b.ResolveUsing(c => c.position))
                .ForMember(a => a.CoordinateY, b => b.ResolveUsing(c => c.row))
                .Ignore(a => a.IsInstructor);

            Mapper.CreateMap<booking, ScheduledClass>()
                .ForMember(a=>a.StartTime,b=>b.ResolveUsing(c=>   c.date.FromApiDateTimeString(c.startTime)))
                .ForMember(a => a.EndTime, b => b.ResolveUsing(c => c.date.FromApiDateTimeString(c.endTime)))
                .ForMember(a => a.BookingId, b => b.ResolveUsing(c => new Identity(c.bookingId.center, c.bookingId.id)))
            .ForMember(a => a.ActivityId, b => b.ResolveUsing(c => c.activity.activityId));

            Mapper.CreateMap<friend, PersonFriend>().ForMember(a => a.FriendId, b => b.ResolveUsing(c => new Identity(c.personKey.center, c.personKey.id)));

            Mapper.CreateMap<clipcard, PurchasedClipCard>()
                .ForMember(a=>a.ValidUntilDate,b=>b.ResolveUsing(c=> c.validUntilDate.FromApiDateString()))
                .Ignore(a=>a.Region);

            Mapper.CreateMap<participation, Participation>()
                .ForMember(a => a.ScheduledClass, b => b.ResolveUsing(c => c.booking));
                
            Mapper.CreateMap<PurchaseDetails, sellClipcardParameters>().ConstructUsing(
                s =>
                    new sellClipcardParameters()
                    {
                        campaignCode = s.CampaignCode,
                        paymentInfo = new WebServices.SubscriptionAPI.paymentInfo()
                        {
                            amountPaidByCustomer = s.AmountPaid.ToString("###0.00"),
                            paymentMethod = paymentMethod.CASH_ACCOUNT,
                            creditCardTransactionRef = s.SageTxAuthNo
                        },
                        personId = Mapper.Map<WebServices.SubscriptionAPI.compositeKey>(s.PersonId),
                        productId = Mapper.Map<WebServices.SubscriptionAPI.compositeKey>(s.ProductId)
                    })
                    .Ignore(a => a.paymentInfo);

            Mapper.CreateMap<availableClipcard, AvailableClipcard>().ForMember(a => a.ProductId, b => b.ResolveUsing(c => Mapper.Map<WebServices.SubscriptionAPI.compositeKey>(c.productId)));

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
