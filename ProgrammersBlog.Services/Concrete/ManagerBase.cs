using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ProgrammersBlog.Data.Abstract;

namespace ProgrammersBlog.Services.Concrete
{
    public class ManagerBase
    {
        public ManagerBase(IMapper mapper, IUnitOfWork unitOfWork)
        {
            Mapper = mapper;
            UnitOfWork = unitOfWork;
        }

        protected IMapper Mapper { get; }
        protected IUnitOfWork UnitOfWork { get; }

    }
}
