﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace API.Helpers
{
    public static class TreeExtensions
    {
        public class TreeItem<T>
        {
            public T Item { get; set; }
            public IEnumerable<TreeItem<T>> Children { get; set; }
        }

        /// <summary>
        /// Generates tree of items from item list
        /// </summary>
        /// 
        /// <typeparam name="T">Type of item in collection</typeparam>
        /// <typeparam name="K">Type of parent_id</typeparam>
        /// 
        /// <param name="collection">Collection of items</param>
        /// <param name="id_selector">Function extracting item's id</param>
        /// <param name="parent_id_selector">Function extracting item's parent_id</param>
        /// <param name="root_id">Root element id</param>
        /// 
        /// <returns>Tree of items</returns>
        public static IEnumerable<TreeItem<T>> GenerateTree<T, K>(
            this IEnumerable<T> collection,
            Func<T, K> id_selector,
            Func<T, K> parent_id_selector,
            K root_id = default)
        {
            foreach (var c in collection.Where(c => EqualityComparer<K>.Default.Equals(parent_id_selector(c), root_id)))
            {
                yield return new TreeItem<T>
                {
                    Item = c,
                    Children = collection.GenerateTree(id_selector, parent_id_selector, id_selector(c))
                };
            }
        }
        ///// <summary> Generic interface for tree node structure </summary>
        ///// <typeparam name="T"></typeparam>
        //public interface ITree<T>
        //{
        //    T Data { get; }
        //    ITree<T> Parent { get; }
        //    ICollection<ITree<T>> Children { get; }
        //    bool IsRoot { get; }
        //    bool IsLeaf { get; }
        //    int Level { get; }
        //}
        ///// <summary> Flatten tree to plain list of nodes </summary>
        //public static IEnumerable<TNode> Flatten<TNode>(this IEnumerable<TNode> nodes, Func<TNode, IEnumerable<TNode>> childrenSelector)
        //{
        //    if (nodes == null) throw new ArgumentNullException(nameof(nodes));
        //    return nodes.SelectMany(c => childrenSelector(c).Flatten(childrenSelector)).Concat(nodes);
        //}
        ///// <summary> Converts given list to tree. </summary>
        ///// <typeparam name="T">Custom data type to associate with tree node.</typeparam>
        ///// <param name="items">The collection items.</param>
        ///// <param name="parentSelector">Expression to select parent.</param>
        //public static ITree<T> ToTree<T>(this IList<T> items, Func<T, T, bool> parentSelector)
        //{
        //    if (items == null) throw new ArgumentNullException(nameof(items));
        //    var lookup = items.ToLookup(item => items.FirstOrDefault(parent => parentSelector(parent, item)),
        //        child => child);
        //    return Tree<T>.FromLookup(lookup);
        //}
        //public static List<T> GetParents<T>(ITree<T> node, List<T> parentNodes = null) where T : class
        //{
        //    while (true)
        //    {
        //        parentNodes ??= new List<T>();
        //        if (node?.Parent?.Data == null) return parentNodes;
        //        parentNodes.Add(node.Parent.Data);
        //        node = node.Parent;
        //    }
        //}
        ///// <summary> Internal implementation of <see cref="ITree{T}" /></summary>
        ///// <typeparam name="T">Custom data type to associate with tree node.</typeparam>
        //internal class Tree<T> : ITree<T>
        //{
        //    public T Data { get; }
        //    public ITree<T> Parent { get; private set; }
        //    public ICollection<ITree<T>> Children { get; }
        //    public bool IsRoot => Parent == null;
        //    public bool IsLeaf => Children.Count == 0;
        //    public int Level => IsRoot ? 0 : Parent.Level + 1;
        //    private Tree(T data)
        //    {
        //        Children = new LinkedList<ITree<T>>();
        //        Data = data;
        //    }
        //    public static Tree<T> FromLookup(ILookup<T, T> lookup)
        //    {
        //        var rootData = lookup.Count == 1 ? lookup.First().Key : default(T);
        //        var root = new Tree<T>(rootData);
        //        root.LoadChildren(lookup);
        //        return root;
        //    }
        //    private void LoadChildren(ILookup<T, T> lookup)
        //    {
        //        foreach (var data in lookup[Data])
        //        {
        //            var child = new Tree<T>(data) { Parent = this };
        //            Children.Add(child);
        //            child.LoadChildren(lookup);
        //        }
        //    }
        //}
    }
}
