using Umbraco.Web;
using Umbraco.Web.Models;

namespace PureRide.Web.Helpers
{
    public class RenderModel<T> : RenderModel
    {
        private readonly T _baseModel;

        public T PureRide
        {
            get { return _baseModel; }
        }

        public RenderModel(T baseModel)
            : base(UmbracoContext.Current.PublishedContentRequest.PublishedContent)
        {
            _baseModel = baseModel;
        }
    
    }
}