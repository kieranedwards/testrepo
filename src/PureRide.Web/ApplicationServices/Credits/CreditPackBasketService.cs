using System;
using System.Web;
using Exerp.Api.DataTransfer;
using Exerp.Api.Interfaces.Services;
using PureRide.Data.DataTransfer;
using PureRide.Data.Repositories;
using PureRide.Web.ApplicationServices.Account;
using PureRide.Web.Providers;

namespace PureRide.Web.ApplicationServices.Credits
{
    public interface ICreditPackBasketService
    {
        string SetSelectedClipCard(string productId, string campaignCode);
        ClipCardOrder GetSelectedClipCard();
        bool HasSelectedClipCard();
        void ClearSelectedClipCard();
    }

    public class CreditPackBasketService : ICreditPackBasketService
    {
        private readonly IHttpContextProvider _httpContextProvider;
        private readonly IAuthenticationService _authenticationService;
        private readonly IClipCardPurchaseService _clipCardPurchaseService;
        private readonly IClipCardRepository _clipCardRepository;
        public const string PendingPurchaseCookieName = "Basket";
        public const string OrderIdKey = "orderId";
        
        public CreditPackBasketService(IHttpContextProvider httpContextProvider,
                                        IAuthenticationService authenticationService,
                                        IClipCardPurchaseService clipCardPurchaseService,
                                        IClipCardRepository clipCardRepository)
        {
            _httpContextProvider = httpContextProvider;
            _authenticationService = authenticationService;
            _clipCardPurchaseService = clipCardPurchaseService;
            _clipCardRepository = clipCardRepository;
        }

        public string SetSelectedClipCard(string productId, string campaignCode)
        {
            var user = _authenticationService.GetCurrentUser();

            if (user == null)
            {
                var returnUrl = _httpContextProvider.Current.Request.UrlReferrer == null ? "" : HttpUtility.UrlEncode(_httpContextProvider.Current.Request.UrlReferrer.PathAndQuery);
                return string.Concat("/account/login/?returnUrl=", returnUrl);
            }

            //We must fetch it again, as we cannot trust the user to post the price to us
            var product = new Identity(productId);
            var clipcard = _clipCardPurchaseService.GetClipPack(_authenticationService.GetCurrentUser(), product, campaignCode);

            var id = _clipCardRepository.CreateOrder(_authenticationService.GetCurrentUser(), product, clipcard.Name, clipcard.Price, campaignCode);
            var cookie = MakeCookie(id);
            _httpContextProvider.Current.Response.Cookies.Add(cookie);
           
            return "/credits/billingaddress/";
        }

        public ClipCardOrder GetSelectedClipCard()
        {
            var data = _httpContextProvider.Current.Request.Cookies[PendingPurchaseCookieName];
            return data == null ? null : _clipCardRepository.GetBasket(new Guid(data[OrderIdKey]));
        }

        public bool HasSelectedClipCard()
        {
            var cookie = _httpContextProvider.Current.Request.Cookies[PendingPurchaseCookieName];
            return cookie != null;
        }

        private HttpCookie MakeCookie(Guid orderId)
        {
            var cookie = new HttpCookie(PendingPurchaseCookieName)
            {
                Expires = DateTime.Now.AddHours(1),
                HttpOnly = true,
                Shareable = false,
                Secure = _httpContextProvider.Current.Request.IsSecureConnection
            };

            cookie.Values[OrderIdKey] = orderId.ToString();
            return cookie;
        }
         
        public void ClearSelectedClipCard()
        {
            var cookie = new HttpCookie(PendingPurchaseCookieName){Expires = DateTime.Now.AddHours(-1)};
            _httpContextProvider.Current.Response.Cookies.Add(cookie);
        }
    }
}
