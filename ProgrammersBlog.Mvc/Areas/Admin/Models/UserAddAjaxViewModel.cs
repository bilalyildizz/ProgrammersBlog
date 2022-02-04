using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProgrammersBlog.Entities.Dtos;

namespace ProgrammersBlog.Mvc.Areas.Admin.Models
{
    //BU sınıfı sadece mvc katmanında kullanıyoruz. 
    public class UserAddAjaxViewModel
    {
        //Bu özellik categry eklemek için hata olduğunda dönüyoruz hatayı gösterip inputların içini dolu göstermek için.
        public UserAddDto UserAddDto { get; set; }

        // Bu partialı dönme nedenimiz validatiion işlemlerimizde bir hata olduğunda hatayı gösterirken tekrar viewıda dönmeliyiz.
        // string olarak dönme nedenimiz içindeki modelide almalıyız.
        public string UserAddPartial { get; set; }
        //Ajax işlemleri için bize eklenen kategorinin geri dönmesi gerekiyordu. İŞlem başarılıysa döndürüyoruz tabloya ekleme işlemi yapılıyor .
        public UserDto UserDto { get; set; }

    }
}
