using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.DTO.AccountDTO.Common
{
    public class ApiResponseModel
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public object Data { get; set; } //Chưa biết nó sẽ trả về list hay object hay gì nên để làm object
    }
}
