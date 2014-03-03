namespace Utilities.IO.Sync
{
    public class NameValuePair<T, T1>
    {
        public NameValuePair(T key, T1 value)
        {
            Name = key;
            Value = value;
        }

        public T Name { get; set; }
        public T1 Value { get; set; }
    }
}