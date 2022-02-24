using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProgrammersBlog.Entities.Dtos;

namespace ProgrammersBlog.Mvc.Areas.Admin.Models
{
    //BU sınıfı sadece mvc katmanında kullanıyoruz. 
    public class CategoryAddAjaxViewModel
    {
        //Bu özellik categry eklemek içi
        public CategoryAddDto CategoryAddDto { get; set; }

        // Bu partialı dönme nedenimiz validatiion işlemlerimizde bir hata olduğunda hatayı gösterirken tekrar viewıda dönmeliyiz.
        // string olarak dönme nedenimiz içindeki modelide almalıyız.
        public string CategoryAddPartial { get; set; }
        //Ajax işlemleri için bize eklenen kategorinin geri dönmesi gerekiyordu.
        public CategoryDto CategoryDto { get; set; }

    }
}
