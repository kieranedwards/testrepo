using System;
using System.Linq;
using System.Web.Routing;
using PureRide.Web;
using PureRide.Web.Helpers;
using umbraco;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace PureRide.WebUI.Routes
{
    /// <summary>
    /// Allows to set the correct view model when using a custom route
    /// </summary>
    public class PublishedPageRouteHandler :  UmbracoVirtualNodeRouteHandler
    {
        private readonly int _pageId;

        public PublishedPageRouteHandler(WellKnownPage pageId)
        {
            _pageId = (int)pageId;
        }
   
        protected override IPublishedContent FindContent(RequestContext requestContext, UmbracoContext umbracoContext)
        {
            return umbracoContext.ContentCache.GetById(_pageId);
            
            //var helper = new UmbracoHelper(umbracoContext);
            //helper.TypedContent(_pageId);
        }
    }


    public class StudioLocationRouteHandler : UmbracoVirtualNodeRouteHandler
    {
        
        protected override IPublishedContent FindContent(RequestContext requestContext, UmbracoContext umbracoContext)
        {

            if(!requestContext.RouteData.Values.ContainsKey("location"))
                throw new Exception("Location needs to be provided");

            var location = requestContext.RouteData.Values["location"].ToString().FromStudioSlug();
            var node = uQuery.GetNodesByName(location).Where(a=>a.Parent.Id == (int)WellKnownPage.LocationPage).ToArray();

            int pageId;
            if (node.Any())
                pageId = node.First().Id;
            else
                pageId = (int) WellKnownPage.LocationDefaultPage;
            
            return umbracoContext.ContentCache.GetById(pageId);    
        }
    }


}