using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgrammersBlog.Shared.Entities.Concrete;
using ProgrammersBlog.Shared.Utilities.Results.ComplexTypes;

namespace ProgrammersBlog.Shared.Utilities.Results.Abstract
{
    public interface IResult
    {
        public ResultStatus ResultStatus { get; } //ResultStatus.Succes //ResultStatus.Error
        public string Message { get; }
        public Exception Exception { get; }
        //IEnumerable yapma nedenimiz daha sonradan ValidationErrors.Add şeklinde ekleme yapılmaması. Başta oluşturulduğu gibi kalıyor.
        public IEnumerable<ValidationError> ValidationErrors { get; set; }
    }
}
