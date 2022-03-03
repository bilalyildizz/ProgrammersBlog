using System;
using System.Data.SqlTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProgrammersBlog.Shared.Entities.Concrete;

namespace ProgrammersBlog.Mvc.Filters
{
    //Herhangi bir exception aldığımızda gösterilecek hatanın özelleştirmek için IEXceptionFilter interfacini kullanıyoruz.
    public class MvcExceptionFilter:IExceptionFilter
    {
        //Şu anda hangi ortamda olduğumuzu belirlemek için kulanıyoruz.  Geliştirici ortamımı, ürün kullanıcıya sunulmuş mu.
        private readonly IHostEnvironment _environment;
        private readonly ILogger _logger;
        private readonly IModelMetadataProvider _metadataProvider;

        public MvcExceptionFilter(IHostEnvironment environment, IModelMetadataProvider metadataProvider, ILogger<MvcExceptionFilter> logger)
        {
            _environment = environment;
            _metadataProvider = metadataProvider;
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            //Geliştirici moddaysa işlemleri yap diyoruz görmek için fakat normalde isproduction olmalı.
            if (_environment.IsDevelopment())
            {
                context.ExceptionHandled = true;
                //Shared//Entities içerisinde biz oluşturduk.
                var mvcErrorModel = new MvcErrorModel();
                ViewResult result;
                switch (context.Exception)
                {
                    case SqlNullValueException:
                        mvcErrorModel.Message = "Üzgünüz, işleminiz sırasında beklenmedik bir veritabanı hatası oluştu. Sorunu en kısa sürede çözeceğiz.";
                        mvcErrorModel.Detail = context.Exception.Message;
                        result = new ViewResult {ViewName = "Error"};
                        result.StatusCode = 500;
                        _logger.LogError(context.Exception,context.Exception.Message);
                        break;
                    case NullReferenceException:
                        mvcErrorModel.Message = "Üzgünüz, işleminiz sırasında beklenmedik bir bull veriye rastlandı. Sorunu en kısa sürede çözeceğiz.";
                        result = new ViewResult { ViewName = "Error" };
                        mvcErrorModel.Detail = context.Exception.Message;
                        result.StatusCode = 500;
                        _logger.LogError(context.Exception, context.Exception.Message);
                        break;
                    default:
                        mvcErrorModel.Message =
                            "Üzgünüz, işleminiz sırasında beklenmedik bir hata oluştu. Sorunu en kısa sürede çözeceğiz.";
                        result = new ViewResult { ViewName = "Error" };
                        mvcErrorModel.Detail = context.Exception.Message;
                        result.StatusCode = 500;
                        _logger.LogError(context.Exception, context.Exception.Message);
                        break;
                }
                
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
