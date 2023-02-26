using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SRLCore.Model.Response.Kavenagar
{  
    public class KavenegarObjectResponse<T>
    {
        public Return @return { get; set; }
        public T entries { get; set; } 

        public class Return
        {
            public int status { get; set; }
            public string message { get; set; }
        }
    } 
}
