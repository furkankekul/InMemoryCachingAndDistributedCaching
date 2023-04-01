using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;

namespace DistributedCaching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        #region Distributed Cache İşlem Sırası
        //İlk olarak uygulamaya StackExchangesRedis kütüphaesini indiriyoruz.
        //Daha sonra AddStackExchangesRedisCache servisini uygulamaya ekliyoruz.
        //Daha sonra IDistributedCache referansını DependencyIncetion ile injekt ediyoruz.
        //SetString metotu ile metinsel,Set metotu ile binary olarak verileriniz redis'e cacheleyebilirisiniz.
        //GetString ve Get metotu ile cachelenmiş verileri elde edebilirsiniz.
        //Remove Fonksiyonu ile cache'lenmiş verileri silebilirsiniz veya sliding time ve absolute time konfigürasyonları yaparak cachede ki veriye bir zaman aralığı sunara manuel değil de 
        //belirlediğiniz zaman aralığında silinmesini sağlayabilirsiniz.
        #endregion
        readonly IDistributedCache _distributedCache;
        public ValuesController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        [HttpGet("set")]
        public void Set(string name)
        {
            byte[] value = Encoding.UTF8.GetBytes(name);
            _distributedCache.Set("NAME", value, options: new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(60),//Data maximum 60 saniye cache'de kalacak sonra kesinlikle silinecek
                SlidingExpiration = TimeSpan.FromSeconds(10) // Data 10 saniye içinde bir istek gelirse cache de kalma süresi uzayacaka ama max 60 saniye uzar 
                //Eğer ki 10 sn içerisinde bir istek gelmezse data cache den kesinlikles silinecek.

            });
        }

        [HttpGet("get")]
        public byte[] Get()
        {
            return _distributedCache.Get("NAME");
        }





    }
}
