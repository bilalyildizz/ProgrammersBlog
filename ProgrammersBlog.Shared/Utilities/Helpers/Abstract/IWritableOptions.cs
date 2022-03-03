using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace ProgrammersBlog.Shared.Utilities.Helpers.Abstract
{
    //Bu interface appsetting vs json dosyaları üzerinde yapmak itediğimiz değişiklikleri direk controllerlar üzerinden yapmak için kullanıyoruz.
    public interface IWritableOptions<out T> : IOptionsSnapshot<T> where T : class, new()
    {
        void Update(Action<T> applyChanges);
    }
}
