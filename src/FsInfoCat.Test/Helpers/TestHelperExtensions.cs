using FsInfoCat.PS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace FsInfoCat.Test.Helpers
{
    public static class TestHelperExtensions
    {
        public static XmlElement AppendElement(this XmlDocument xmlDocument, string localName) => (XmlElement)xmlDocument.AppendChild(xmlDocument.CreateElement(localName));

        public static XmlElement AppendElement(this XmlElement element, string localName) => (XmlElement)element.AppendChild(element.OwnerDocument.CreateElement(localName));

        public static XmlElement SetAttributeValue(this XmlElement element, string localName, string value)
        {
            XmlAttribute attribute = element.GetAttributeNode(localName);
            if (value is null)
            {
                if (!(attribute is null))
                    element.Attributes.Remove(attribute);
            }
            else if (attribute is null)
                element.Attributes.Append(element.OwnerDocument.CreateAttribute(localName)).Value = value;
            else
                attribute.Value = value;
            return element;
        }

        public static XmlElement SetAttributeValue(this XmlElement element, string localName, bool value) => SetAttributeValue(element, localName, XmlConvert.ToString(value));

        public static string SerializesAsXml(Match match, params string[] groupNames)
        {
            XmlDocument xmlDocument = new XmlDocument();
            IEnumerable<Group> groups = match.Groups.Cast<Group>();
            xmlDocument.AppendChild(xmlDocument.CreateElement("Match")).Attributes.Append(xmlDocument.CreateAttribute("Success")).Value = XmlConvert.ToString(match.Success);
            if (match.Success && !(groupNames is null || groupNames.Length == 0))
            {
                foreach (Group g in match.Groups)
                {
                    if (groupNames.Contains(g.Name))
                    {
                        XmlElement element = (XmlElement)xmlDocument.DocumentElement.AppendChild(xmlDocument.CreateElement("Group"));
                        element.Attributes.Append(xmlDocument.CreateAttribute("Success")).Value = XmlConvert.ToString(g.Success);
                        if (g.Success)
                            element.InnerText = g.Value;
                        else
                            element.IsEmpty = true;
                    }
                }
            }
            return xmlDocument.OuterXml;
        }

        public static Func<T1, T2, T3, TResult> Monitor<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> target, out Func<IFuncInvocationResult<TResult>> getResult)
        {
            FunctionInvocationMonitor<TResult> monitor = new FunctionInvocationMonitor<TResult>();
            getResult = monitor.ToResult;
            return monitor.CreateProxy(target);
        }

        public static Action<T1, T2, T3> Monitor<T1, T2, T3>(this Action<T1, T2, T3> target, out Func<IInvocationResult<T1, T2, T3>> getResult)
        {
            InvocationMonitor<T1, T2, T3> monitor = new InvocationMonitor<T1, T2, T3>();
            getResult = monitor.ToResult;
            return monitor.CreateProxy(target);
        }


        public static TryCoerceHandler<T, TOut> Monitor<T, TOut>(this TryCoerceHandler<T, TOut> target, out Func<IFuncInvocationResult<TOut, bool>> getResult)
        {
            FunctionInvocationMonitor<TOut, bool> monitor = new FunctionInvocationMonitor<TOut, bool>();
            getResult = monitor.ToResult;
            return (T t, out TOut o) =>
            {
                bool result = target(t, out o);
                monitor.Apply(result, o);
                return result;
            };
        }

        public static Func<T1, T2, TResult> Monitor<T1, T2, TResult>(this Func<T1, T2, TResult> target, out Func<IFuncInvocationResult<TResult>> getResult)
        {
            FunctionInvocationMonitor<TResult> monitor = new FunctionInvocationMonitor<TResult>();
            getResult = monitor.ToResult;
            return monitor.CreateProxy(target);
        }

        public static Action<T1, T2> Monitor<T1, T2>(this Action<T1, T2> target, out Func<IInvocationResult<T1, T2>> getResult)
        {
            InvocationMonitor<T1, T2> monitor = new InvocationMonitor<T1, T2>();
            getResult = monitor.ToResult;
            return monitor.CreateProxy(target);
        }

        public static Func<T, TResult> Monitor<T, TResult>(this Func<T, TResult> target, out Func<IFuncInvocationResult<TResult>> getResult)
        {
            FunctionInvocationMonitor<TResult> monitor = new FunctionInvocationMonitor<TResult>();
            getResult = monitor.ToResult;
            return monitor.CreateProxy(target);
        }

        public static Action<T> Monitor<T>(this Action<T> target, out Func<IInvocationResult<T>> getResult)
        {
            InvocationMonitor<T> monitor = new InvocationMonitor<T>();
            getResult = monitor.ToResult;
            return monitor.CreateProxy(target);
        }

        public static Func<TResult> Monitor<TResult>(this Func<TResult> target, out Func<IFuncInvocationResult<TResult>> getResult)
        {
            FunctionInvocationMonitor<TResult> monitor = new FunctionInvocationMonitor<TResult>();
            getResult = monitor.ToResult;
            return monitor.CreateProxy(target);
        }

        public static Action Monitor(this Action target, out Func<IInvocationResult> getResult)
        {
            InvocationMonitor monitor = new InvocationMonitor();
            getResult = monitor.ToResult;
            return monitor.CreateProxy(target);
        }
    }
}
