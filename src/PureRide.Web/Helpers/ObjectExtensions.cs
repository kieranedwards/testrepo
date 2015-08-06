using System.Linq;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace PureRide.Web.Helpers
{

    public static class ObjectExtensions
    {

        /// <summary>
        /// Used to Warp a normal model in a render model so we have access to both Umbraco and our own models
        /// </summary>
        /// <typeparam name="T">The Model Type</typeparam>
        /// <param name="baseModel">Model to Wrap</param>
        /// <returns></returns>
        /// <remarks>Needed so the same model can be used by umbraco methods and API methoids that do not have an umbraco context</remarks>
        public static RenderModel<T> AsRenderModel<T>(this T baseModel)
        {
            return new RenderModel<T>(baseModel);
        }

        /// <summary>
        /// See http://stackoverflow.com/a/17509867/33
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page"></param>
        /// <remarks>Needed as CurrentUmbracoPage does not keep the model</remarks>
        public static void RetrieveModel<T>(this WebViewPage<T> page) where T : class
        {
            var models = page.ViewContext.TempData.Where(item => item.Value is T).ToArray();

            if (models.Any())
            {
                page.ViewData.Model = (T)models.First().Value;
                page.ViewContext.TempData.Remove(models.First().Key);
            }
        }

        public static UmbracoPageResult WithModel<T>(this UmbracoPageResult page, TempDataDictionary tempData, T model) where T : class
        {
            if (tempData.ContainsKey("CustomModel"))
                tempData["CustomModel"] = model;
            else
                tempData.Add("CustomModel", model);

            return page;
        }
       
        
    }
}
