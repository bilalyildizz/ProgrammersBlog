using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Hosting;
using ProgrammersBlog.Shared.Entities.Concrete;

namespace ProgrammersBlog.Mvc.Filters
{
    //Herhangi bir exception aldığımızda gösterilecek hatanın özelleştirmek için IEXceptionFilter interfacini kullanıyoruz.
    public class MvcExceptionFilter:IExceptionFilter
    {
        //Şu anda hangi ortamda olduğumuzu belirlemek için kulanıyoruz.  Geliştirici ortamımı, ürün kullanıcıya sunulmuş mu.
        private readonly IHostEnvironment _environment;
        private readonly IModelMetadataProvider _metadataProvider;

        public MvcExceptionFilter(IHostEnvironment environment, IModelMetadataProvider metadataProvider)
        {
            _environment = environment;
            _metadataProvider = metadataProvider;
        }

        public void OnException(ExceptionContext context)
        {
            //Geliştirici moddaysa işlemleri yap diyoruz görmek için fakat normalde isproduction olmalı.
            if (_environment.IsDevelopment())
            {
                context.ExceptionHandled = true;
                //Shared//Entities içerisinde biz oluşturduk.
                var mvcErrorModel = new MvcErrorModel
                {
                    Message = "Üzgünüz, işleminiz sırasında beklenmedik bir hata oluştu. Sorunu en kısa sürede çözeceğiz."
                };
                var result = new ViewResult {ViewName = "Error"};
                result.StatusCode = 500;
                result.ViewData = new ViewDataDictionary(_metadataProvider, context.ModelState);
                //eklediğimiz viewdata ya bir key veriyoruz error viewda kullanmak için ve modeli gönderiyoruz.
                result.ViewData.Add("MvcErrorModel",mvcErrorModel);
                context.Result = result;
                //Startup içerisinde eklenmeli.
            }
        }
    }
}
