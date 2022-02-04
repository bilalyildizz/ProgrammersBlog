using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammersBlog.Data.Abstract
{
    
    public interface IUnitOfWork:IAsyncDisposable
    {
        IArticleRepository Articles { get; }  //unitOfWork.Articles
        ICategoryRepository Categories { get; }//_unitOfWork.Categories.AddAsync();
        ICommentRepository Comments { get; }

        //_unitOfWork.Categories.AddAsync(category)
        //_unitOfWork.Users.AddAsync(user)
        //_unitOfWork.SaveAsync()


        //Repository kısmında hiç veritabanına ekklediklerimiz vs için save yapmadık  bu yüzden burada eklliyoruz. Üstteki örnekteki gibi kullanılıyor
        // İnt dönme nedeni bu save işleminden kaç verinin etkilendiğini görmek isteyebiliriz.
        Task<int> SaveAsync();


    }
}
