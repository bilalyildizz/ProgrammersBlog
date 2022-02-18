using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgrammersBlog.Shared.Utilities.Results.ComplexTypes;

namespace ProgrammersBlog.Shared.Entities.Abstract
{
    //Get işlemleri için kullanılacak dto lar için kullanılacak. Add vs işlemlerde zaten result döndüğü için kullanmayacağız.
    public abstract class DtoGetBase
    {
        public virtual ResultStatus ResultStatus { get; set; }
        public virtual string Message { get; set; }
        public virtual int CurrentPage { get; set; }
        public virtual int PageSize { get; set; }
        public virtual int TotalCount { get; set; }
        public virtual int TotalPages => (int) Math.Ceiling(decimal.Divide(TotalCount, PageSize));
        public virtual bool ShowPrevious => CurrentPage > 1;
        public virtual bool ShowNext => CurrentPage<TotalPages;
        public virtual bool IsAscending { get; set; } = false;
    }
}
