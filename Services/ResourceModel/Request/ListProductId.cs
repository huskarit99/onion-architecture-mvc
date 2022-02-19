using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ResourceModel.Request
{
    public class DeleteManyByIdRequest
    {
        public List<int> ListProductId { get; set; }
    }
}
