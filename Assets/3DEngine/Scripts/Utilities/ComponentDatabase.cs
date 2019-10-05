using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//source: https://answers.unity.com/questions/1549215/getting-all-component-types-even-those-not-on-the.html
public static class ComponentDatabase
{
    public class TypeNode : IEnumerable<System.Type>
    {
        public System.Type type;
        public TypeNode next = null;
        public IEnumerator<System.Type> GetEnumerator()
        {
            for (var t = this; t != null; t = t.next)
                yield return t.type;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    private static List<System.Type> m_Types;
    private static Dictionary<string, TypeNode> m_Dict;
    static ComponentDatabase()
    {
        var comp = typeof(Component);
        var hashset = new HashSet<System.Type>();
        m_Dict = new Dictionary<string, TypeNode>();
        foreach (var a in System.AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (var t in a.GetTypes())
            {
                if (comp.IsAssignableFrom(t) && !t.IsAbstract && !hashset.Contains(t) && t != comp)
                {
                    hashset.Add(t);
                    TypeNode tn = null;
                    m_Dict.TryGetValue(t.Name, out tn);
                    tn = new TypeNode { next = tn, type = t };
                    m_Dict[t.Name] = tn;
                }
            }
        }
        m_Types = new List<System.Type>(hashset.Count);
        m_Types.AddRange(hashset);
    }
    public static TypeNode FindComponent(string aComponentName)
    {
        TypeNode tn;
        if (m_Dict.TryGetValue(aComponentName, out tn))
            return tn;
        return null;
    }
    public static List<System.Type> GetTypes(System.Type aBaseType)
    {
        var res = new List<System.Type>();
        foreach (var t in m_Types)
        {
            if (aBaseType.IsAssignableFrom(t))
                res.Add(t);
        }
        return res;
    }
    public static List<System.Type> GetTypes<T>()
    {
        return GetTypes(typeof(T));
    }
    public static IEnumerable<System.Type> GetAllTypes()
    {
        return m_Types.AsReadOnly();
    }

    //added
    public static string[] GetAllComponentAssemblyNames(bool _sortByComponentName = false)
    {
        List<string> assemblyNames = new List<string>(new string[m_Types.Count]);
        List<string> componentNames = new List<string>(new string[m_Types.Count]);
        Dictionary<int, string> assKeys = new Dictionary<int, string>();
        Dictionary<int, string> compKeys = new Dictionary<int, string>();
        for (int i = 0; i < m_Types.Count; i++)
        {
            assemblyNames[i] = m_Types[i].AssemblyQualifiedName;
            componentNames[i] = m_Types[i].Name;
            //need to add component names to dictionarys for sorting and re-linking
            assKeys.Add(i, assemblyNames[i]);
            compKeys.Add(i, componentNames[i]);
        }

        if (_sortByComponentName)
        {
            List<string> assemblyNamesSorted = new List<string>();
            //order component names by value, alphabetically
            var ordered = compKeys.OrderBy(x => x.Value);
            foreach (var compKey in ordered)
            {
                string val;
                //match component key to assembly dictionary key to get proper component order
                assKeys.TryGetValue(compKey.Key, out val);
                assemblyNamesSorted.Add(val);
            }
            return assemblyNamesSorted.ToArray();
        } 

        return assemblyNames.ToArray();
    }

    public static string[] GetAllComponentNames(bool _sort = false)
    {
        List<string> typeNames = new List<string>(new string[m_Types.Count]);
        for (int i = 0; i < m_Types.Count; i++)
        {
            typeNames[i] = m_Types[i].Name;
        }
        if (_sort)
            typeNames.Sort();
        return typeNames.ToArray();
    }
}
