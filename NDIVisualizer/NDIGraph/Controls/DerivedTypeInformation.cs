namespace Reflector.Browser
{
	using System;
	using System.Collections;
	using System.IO;
	using Reflector.CodeModel;

    //internal class DerivedTypeInformation
    //{
    //    private IDictionary table;

    //    public DerivedTypeInformation(IAssemblyManager AssemblyManager, IVisibilityConfiguration visibility)
    //    {
    //        this.table = new Hashtable();

    //        IAssembly[] assemblies = new IAssembly[AssemblyManager.Assemblies.Count];
    //        AssemblyManager.Assemblies.CopyTo(assemblies, 0);

    //        TypeEnumerator enumerator = new TypeEnumerator(assemblies);
    //        while (enumerator.MoveNext())
    //        {
    //            ITypeDeclaration typeDeclaration = (ITypeDeclaration) enumerator.Current;
    //            if (Helper.IsVisible(typeDeclaration, visibility))
    //            {
    //                ITypeReference baseType = typeDeclaration.BaseType;
    //                if (baseType != null)
    //                {
    //                    this.AddToTable(baseType, typeDeclaration);
    //                }

					
    //                foreach (ITypeReference interfaceType in typeDeclaration.Interfaces)
    //                {
    //                    this.AddToTable(interfaceType, typeDeclaration);
    //                }
    //            }
    //        }
    //    }

    //    public IEnumerable GetDerivedTypes(ITypeDeclaration typeDeclaration)
    //    {
    //        ArrayList list = (ArrayList) this.table[typeDeclaration];
    //        if (list == null)
    //        {
    //            return new ArrayList(0);
    //        }

    //        list.Sort();

    //        return list;
    //    }

    //    private void AddToTable(ITypeReference typeReference, ITypeDeclaration typeDeclaration)
    //    {
    //        ArrayList list = (ArrayList)this.table[typeReference];
    //        if (list == null)
    //        {
    //            list = new ArrayList(0);
    //            this.table.Add(typeReference, list);
    //        }

    //        // Check if list already contains typeDeclaration.
    //        for (int i = 0; i < list.Count; i++)
    //        {
    //            if (list[i] == typeDeclaration)
    //            {
    //                return;
    //            }
    //        }

    //        list.Add(typeDeclaration);
    //    }
    //}
}
