using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Data
{
    public interface IForum
    {
        ForumModel GetById(int id);
        IEnumerable<ForumModel> GetAll();

    }
}
