using System;
using System.ComponentModel;

namespace Domain
{
    public class DynamicEntityPropertyDescriptor : PropertyDescriptor
    {
        public DynamicEntityPropertyDescriptor(string name)
            : base(name, null)
        {
        }

        public override object GetValue(object component)
        {
            return ((DynamicEntity)component)[Name];
        }

        public override void SetValue(object component, object value)
        {
            ((DynamicEntity)component)[Name] = (string)value;
        }

        public override void ResetValue(object component)
        {
            ((DynamicEntity)component)[Name] = null;
        }

        public override bool CanResetValue(object component)
        {
            return true;
        }

        public override bool ShouldSerializeValue(object component)
        {
            return ((DynamicEntity)component)[Name] != null;
        }

        public override Type PropertyType
        {
            get { return typeof(string); }
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override Type ComponentType
        {
            get { return typeof(DynamicEntity); }
        }
    }
}