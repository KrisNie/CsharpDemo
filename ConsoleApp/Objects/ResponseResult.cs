using System;

namespace Objects
{
    public class ResponseResult
    {
        /// <summary>
        /// success or not
        /// </summary>
        public bool IsSuccess;

        /// <summary>
        /// timestamp
        /// </summary>
        public long BeginTimeStamp = DateTime.Now.ToFileTime();

        /// <summary>
        /// Time cost
        /// </summary>
        public int TimeCost;

        /// <summary>
        /// error message
        /// </summary>
        public string ErrMsg;

        /// <summary>
        /// data
        /// </summary>
        public object Data;

    }
}
