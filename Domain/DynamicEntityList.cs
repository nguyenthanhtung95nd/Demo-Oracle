using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Domain
{
    public class DynamicEntityList : List<DynamicEntity>, ITypedList
    {
        public DynamicEntity Add(params string[] args)
        {
            if (args == null) throw new ArgumentNullException("args");
            if (args.Length != Columns.Count) throw new ArgumentException("args");
            DynamicEntity bag = new DynamicEntity();
            for (int i = 0; i < args.Length; i++)
            {
                bag[Columns[i]] = args[i];
            }
            Add(bag);
            return bag;
        }

        public DynamicEntityList()
        {
            Columns = new List<string>();
        }

        public List<string> Columns { get; private set; }

        public string GetListName(PropertyDescriptor[] listAccessors)
        {
            return "Foo";
        }

        public PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
        {
            if (listAccessors == null || listAccessors.Length == 0)
            {
                PropertyDescriptor[] props = new PropertyDescriptor[Columns.Count];
                for (int i = 0; i < props.Length; i++)
                {
                    props[i] = new DynamicEntityPropertyDescriptor(Columns[i]);
                }
                return new PropertyDescriptorCollection(props, true);
            }
            throw new NotImplementedException("Relations not implemented");
        }
    }
}