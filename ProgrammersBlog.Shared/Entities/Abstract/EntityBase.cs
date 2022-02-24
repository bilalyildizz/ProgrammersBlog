using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammersBlog.Shared.Entities.Abstract
{

    
    public abstract class  EntityBase
    {

        //bu şekilde değer atarsak  bu sınıfı kullanan sınıflarda bu değeri değiştirmek için override kullanmalıyız. Bunun içinde burada başına virtual yazıyoruz.
        public virtual int Id { get; set; }
        public virtual DateTime CreatedDate { get; set; }=DateTime.Now;
        public virtual DateTime ModifiedDate { get; set; }=DateTime.Now;

        public virtual bool IsDeleted { get; set; } = false;
        public virtual bool IsActive { get; set; } = true;

        public virtual string CreatedByName { get; set; } = "Admin";
        public virtual string ModifiedByName { get; set; } = "Admin";

        //Burada bunu ekleme sebebimiz admn panelinde not tutmal isteyebiliriz eklenen herhangi bir şeyle ilgili.
        public virtual string Note { get; set; }
    }
}
