using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    public class ResponseResult
    {
        /// <summary>
        /// success or not
        /// </summary>
        public bool isSuccess;

        /// <summary>
        /// timestamp
        /// </summary>
        public long beginTimeStamp = DateTime.Now.ToFileTime();

        /// <summary>
        /// Time cost
        /// </summary>
        public int timeCost;

        /// <summary>
        /// error message
        /// </summary>
        public string errMsg;

        /// <summary>
        /// data
        /// </summary>
        public object data;

    }
}
