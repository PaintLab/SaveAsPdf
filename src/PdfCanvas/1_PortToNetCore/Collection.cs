//Apache2, 2017, WinterDev
//Apache2, 2009, griffm, FO.NET
//MIT, 2017, WinterDev
//temp implementation
using System.Collections;
using System.Collections.Generic;

namespace System.Collections.Specialized
{
    public class StringCollection : List<string>
    {

    }
    public struct BitVector32
    {
        int _inner;
        public BitVector32(int innner)
        {
            this._inner = innner;
        }
        public int Data
        {
            get { return _inner; }
        }
        public bool this[int pos]
        {
            get
            {
                return ((_inner >> pos) & 0x1) == 1;
            }
            set
            {
                if (value)
                {
                    this._inner |= (1 << pos);
                }
                else
                {
                    //convert to 
                    this._inner &= ~(1 << pos);
                }
            }
        }
    }
}
namespace System.Collections
{


    public class SortedList : SortedList<object, object>
    {
        public object GetKey(int index)
        {
            return this.Keys[index];
        }
        public SortedList Clone()
        {
            SortedList newclone = new SortedList();
            foreach (var kp in this)
            {
                newclone.Add(kp.Key, kp.Value);
            }
            return newclone;
        }
        public IList GetGetKeyList()
        {
            List<object> list = new List<object>();
            foreach (object k in this.Keys)
            {
                list.Add(k);
            }
            return list;
        }
    }
    public class Hashtable
    {
        Dictionary<object, object> _dic;
        public Hashtable()
        {
            _dic = new Dictionary<object, object>();
        }

        public Hashtable(int cap)
        {
            _dic = new Dictionary<object, object>(cap);
        }
        public bool ContainsKey(object key)
        {
            return _dic.ContainsKey(key);
        }
        public Dictionary<object, object>.Enumerator GetEnumerator()
        {
            return _dic.GetEnumerator();
        }
        public Dictionary<object, object>.KeyCollection Keys
        {
            get { return _dic.Keys; }
        }
        public Dictionary<object, object>.ValueCollection Values
        {
            get { return _dic.Values; }
        }
        public int Count
        {
            get { return _dic.Count; }
        }
        public void Clear()
        {
            _dic.Clear();
        }
        public bool Contains(object key)
        {
            return _dic.ContainsKey(key);
        }
        public object GetValueOrNull(object key)
        {
            object found;
            _dic.TryGetValue(key, out found);
            return found;
        }
        public void Add(object key, object value)
        {
            _dic[key] = value;
            //if (!_dic.ContainsKey(key))
            //{
            //    _dic.Add(key, value);
            //}
            //else
            //{
            //    //replace
            //    _dic[key] = value;
            //}

        }
        public void Remove(object key)
        {
            _dic.Remove(key);
        }
        public object this[object key]
        {
            get
            {
                return GetValueOrNull(key);
            }
            set
            {
                _dic[key] = value;
            }
        }
    }
    public class ArrayList : List<object>
    {
        public ArrayList() { }
        public ArrayList(int initcap)
            : base(initcap)
        {

        }
        public ArrayList(IList<object> objs)
        {
            this.AddRange(objs);
        }
    }
    public static class ArrayListExtensions
    {
        public static T[] ToArray<T>(this ArrayList arr)
        {
            int j = arr.Count;
            T[] convertedArr = new T[j];
            int i = 0;
            foreach (object o in arr)
            {
                convertedArr[i] = (T)o;
                i++;
            }
            return convertedArr;
        }

    }
    public class Stack : Stack<object> { }
}