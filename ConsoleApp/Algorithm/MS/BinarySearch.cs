namespace Algorithm.MS
{
    public class BinarySearch
    {
        public int Search(int[] nums, int target)
        {
            int low = 0, high = nums.Length - 1;
            while (low <= high)
            {
                int mid = (high - low) / 2 + low;
                int num = nums[mid];
                if (num == target)
                {
                    return mid;
                }
                else if (num > target)
                {
                    high = mid - 1;
                }
                else
                {
                    low = mid + 1;
                }
            }

            return -1;
        }
    }
}