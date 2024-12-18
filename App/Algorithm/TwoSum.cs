using System.Collections.Generic;

namespace Algorithm.MS
{
    // https://leetcode-cn.com/problems/two-sum/
    public class TwoSum
    {
        public int[] Sum1(int[] nums, int target)
        {
            for (var i = 0; i < nums.Length; i++)
            {
                for (var j = 0; j < nums.Length; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    if (nums[i] + nums[j] == target)
                    {
                        return new int[] { i, j };
                    }
                }
            }

            return null;
        }

        public int[] Sum2(int[] nums, int target)
        {
            var dic = new Dictionary<int, int>();
            for (var i = 0; i < nums.Length; i++)
            {
                var except = target - nums[i];

                if (dic.ContainsKey(except))
                {
                    return new[] { i, dic[except] };
                }

                if (!dic.ContainsKey(nums[i]))
                {
                    dic.Add(nums[i], i);
                }
            }

            return null;
        }
    }
}