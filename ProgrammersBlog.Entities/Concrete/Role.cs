using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ProgrammersBlog.Shared.Entities.Abstract;

namespace ProgrammersBlog.Entities.Concrete
{
    // Microsoft.Extensions.Identity.Stores eklenmeli ıdentitiyden kalıtım almak için
    //veritabanında id int ile oluşucak id sayesinde.
    public class Role:IdentityRole<int>
    {


    }
}
