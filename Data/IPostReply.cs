using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Data
{
    public interface IPostReply
    {
        PostReply GetById(int id);
        Task Edit(int id, string massage);
    }
}
