using System.Reflection;

namespace BasiaProjektRazorPages.DbModels
{
    public class BaseDbModel
    {
        public override bool Equals(object? other)
        {
            if (other == null)
                return false;
            foreach (PropertyInfo propInfo in this.GetType().GetProperties())
            {
                var val1 = propInfo.GetValue(this);
                var val2 = other.GetType().GetProperty(propInfo.Name).GetValue(other);
                if (val1 == null && val2 == null)
                    continue;
                if (val1 == null && val2 != null)
                    return false;
                if (val1.Equals(val2) == false)
                    return false;
            }
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public void changeNullStringPropertiesToEmptyStrings()
        {
            foreach(PropertyInfo propInfo in this.GetType().GetProperties())
            {
                if (typeof(string).IsAssignableFrom(propInfo.PropertyType) && propInfo.GetValue(this) == null)
                {
                    propInfo.SetValue(this, "");
                }
            }
        }

        public void changeNullPropertiesToDefaultValues(IEnumerable<Type>? typesToMakeDefault = null)
        {
            if (typesToMakeDefault == null)
                typesToMakeDefault = new List<Type> { typeof(string), typeof(int), typeof(float), typeof(double) };
            foreach (PropertyInfo propInfo in this.GetType().GetProperties())
            {
                if (typesToMakeDefault.Contains(propInfo.PropertyType) && propInfo.GetValue(this) == null)
                {
                    Type type = propInfo.PropertyType;
                    if (type == typeof(string))
                        propInfo.SetValue(this, "");
                    else
                        propInfo.SetValue(this, Activator.CreateInstance(type));
                }
            }
        }
    }
}
