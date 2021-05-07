using FsInfoCat.Desktop.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace FsInfoCat.Desktop.Model.ComponentSupport
{
    public sealed class ModelDescriptor<TModel> : IEquatable<ModelDescriptor<TModel>>, IModelDescriptor,
        IReadOnlyDictionary<string, IModelPropertyDescriptor<TModel>>, IEqualityComparer<TModel>
        where TModel : class
    {
        public ReadOnlyCollection<IModelPropertyDescriptor<TModel>> Properties { get; }

        IReadOnlyList<IModelPropertyDescriptor> IModelDescriptor.Properties => Properties;

        IReadOnlyList<IModelProperty> IModelInfo.Properties => Properties;

        Type IModelDescriptor.ComponentType => typeof(TModel);

        public string SimpleName { get; }

        public string FullName { get; }

        public IEnumerable<string> Keys => Properties.Select(p => p.Name);

        public IModelPropertyDescriptor<TModel> this[string key] => Properties.FirstOrDefault(p => p.Name.Equals(key));

        IModelPropertyDescriptor IReadOnlyDictionary<string, IModelPropertyDescriptor>.this[string key] => throw new NotImplementedException();

        int IReadOnlyCollection<KeyValuePair<string, IModelPropertyDescriptor<TModel>>>.Count => Properties.Count;

        int IReadOnlyCollection<KeyValuePair<string, IModelPropertyDescriptor>>.Count => Properties.Count;

        IEnumerable<IModelPropertyDescriptor<TModel>> IReadOnlyDictionary<string, IModelPropertyDescriptor<TModel>>.Values => Properties;

        IEnumerable<IModelPropertyDescriptor> IReadOnlyDictionary<string, IModelPropertyDescriptor>.Values => Properties;

        private ModelDescriptor(string simpleName, string fullName, ReadOnlyCollection<IModelPropertyDescriptor<TModel>> properties)
        {
            SimpleName = simpleName;
            FullName = fullName;
            Properties = properties;
        }

        public static IModelPropertyDescriptor<TModel> CreateProperty(ModelDescriptor<TModel> modelDescriptor, PropertyDescriptor propertyDescriptor)
        {
            return (IModelPropertyDescriptor<TModel>)Activator.CreateInstance(typeof(ModelPropertyDescriptor<,>)
                .MakeGenericType(typeof(TModel), propertyDescriptor.PropertyType), new object[] { modelDescriptor, propertyDescriptor, null });
        }

        public static ModelDescriptor<TModel> Create(Func<PropertyDescriptor, bool> filter = null,
            Func<ModelDescriptor<TModel>, PropertyDescriptor, IModelPropertyDescriptor<TModel>> propertyFactory = null)
        {
            if (propertyFactory is null)
                propertyFactory = CreateProperty;
            Func<PropertyDescriptor, bool> predicate = (filter is null) ?
                new Func<PropertyDescriptor, bool>(pd => !pd.DesignTimeOnly) :
                new Func<PropertyDescriptor, bool>(pd => !pd.DesignTimeOnly && filter(pd));
            Collection<IModelPropertyDescriptor<TModel>> properties = new Collection<IModelPropertyDescriptor<TModel>>();
            Type t = typeof(TModel);
            ModelDescriptor<TModel> modelDescriptor = new ModelDescriptor<TModel>(t.Name, t.FullName, new ReadOnlyCollection<IModelPropertyDescriptor<TModel>>(properties));
            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(t).OfType<PropertyDescriptor>().Where(predicate))
                properties.Add(propertyFactory(modelDescriptor, pd));
            return modelDescriptor;
        }

        public static ModelDescriptor<TModel> Create(Func<ModelDescriptor<TModel>, PropertyDescriptor, IModelPropertyDescriptor<TModel>> propertyFactory) =>
            Create(null, propertyFactory);

        public bool ContainsKey(string key) => Properties.Any(p => p.Name.Equals(key));

        IEnumerator<KeyValuePair<string, IModelPropertyDescriptor<TModel>>> IEnumerable<KeyValuePair<string, IModelPropertyDescriptor<TModel>>>.GetEnumerator() =>
            Properties.Select(p => new KeyValuePair<string, IModelPropertyDescriptor<TModel>>(p.Name, p)).GetEnumerator();

        IEnumerator<KeyValuePair<string, IModelPropertyDescriptor>> IEnumerable<KeyValuePair<string, IModelPropertyDescriptor>>.GetEnumerator() =>
            Properties.Select(p => new KeyValuePair<string, IModelPropertyDescriptor>(p.Name, p)).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            ((IEnumerable)Properties.Select(p => new KeyValuePair<string, IModelPropertyDescriptor<TModel>>(p.Name, p))).GetEnumerator();

        public bool TryGetValue(string key, out IModelPropertyDescriptor<TModel> value)
        {
            value = Properties.FirstOrDefault(p => p.Name.Equals(key));
            return !(value is null);
        }

        bool IReadOnlyDictionary<string, IModelPropertyDescriptor>.TryGetValue(string key, out IModelPropertyDescriptor value)
        {
            value = Properties.FirstOrDefault(p => p.Name.Equals(key));
            return !(value is null);
        }

        public bool Equals(ModelDescriptor<TModel> other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return Properties.Count == other.Properties.Count &&
                Properties.OrderBy(p => p.Name).SequenceEqual(other.Properties.OrderBy(p => p.Name));
        }

        bool IEquatable<IModelDescriptor>.Equals(IModelDescriptor other) => other is ModelDescriptor<TModel> d && Equals(d);

        public override bool Equals(object obj) => obj is ModelDescriptor<TModel> d && Equals(d);

        public override int GetHashCode() =>
            Properties.Select(pd => pd.GetHashCode()).Concat(new int[] { FullName.GetHashCode() }).ToAggregateHashCode();

        public override string ToString()
        {
            if (Properties.Count == 0)
                return $@"{nameof(ModelDescriptor<TModel>)}<{SimpleName}> {{ }}";
            return $@"{nameof(ModelDescriptor<TModel>)}<{SimpleName}> {{\n\t{
                string.Join("\n\t", Properties.Select(pd => pd.PropertyType.ToCsTypeName() + " " + pd.Name + (pd.IsReadOnly ? " { get; }" : " { get; set; }")))
            }\n}}";
        }

        public bool Equals(TModel x, TModel y)
        {
            if (x is null)
                return y is null;
            if (y is null)
                return false;
            if (ReferenceEquals(x, y))
                return true;
            return Properties.All(pd => pd.Equals(pd.GetValue(x), pd.GetValue(y)));
        }

        bool IEqualityComparer.Equals(object x, object y)
        {
            if (x is TModel a)
                return y is TModel b && Equals(a, b);
            if (y is TModel)
                return false;
            return (x is null) ? y is null : x.Equals(y);
        }

        public int GetHashCode(TModel obj) =>
            Properties.Select(pd => pd.GetHashCode(obj)).Concat(new int[] { FullName.GetHashCode() }).ToAggregateHashCode();

        int IEqualityComparer.GetHashCode(object obj)
        {
            if (obj is TModel m)
                return GetHashCode(m);
            return (obj is null) ? 0 : obj.GetHashCode();
        }
    }
}
