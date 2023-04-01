using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryCaching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        #region In-Memory Cache İşlem Sırası
        //ilk olarak uygulamaya AddMemoryCache servisini ekleyiniz(Bu işlemi uygulamanın program.cs sınıfında builder.Services.AddMemoryCache() olarak yap!!)
        //Daha sonra dependency Incetion ile IMemoryCache den bir instance üretiniz
        //Set metotu ile veriyi cacheleyebilir,Get metotuyla cache'lenmiş veriyi elde edebilirsiniz
        //Remove fonksiyonu ile cachelenmiş veriyi silebilirsiniz
        //TryGetValue metotu ile kontrollü bir şekilde cache'den veriyi okuyabilirsiniz.
        //Aşağı da kod üzerinden adımları uygulamalı bir şekilde en basit haliyle In-Memory Caching'i inceleyebilirsin.
        #endregion

        readonly IMemoryCache _memoryCache;
        public ValuesController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet("Set")]
        public void Set(string name)
        {
            _memoryCache.Set("name",name);
        }

        [HttpGet]
        public string Get()
        {
            if (_memoryCache.TryGetValue<string>("name", out string value))
            {
                return _memoryCache.Get<string>("name");
            }
            return string.Empty;    

            
        }


    }
}
