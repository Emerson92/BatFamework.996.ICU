using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace THEDARKKNIGHT.ProcessCore.DataStruct.Tree {
    /****************************************
     * 
     *         Tree Data Struct
     *  
     * ************************************/
    public class BProcessTree<T,K> where T : BProcessUnit<K> where K : BProcessItem
    {
        private BTree<T,K> rootTree;

        /// <summary>
        /// Get the root of tree
        /// </summary>
        public BProcessUnit<K> RootTree {
            set {
                if (rootTree == null)
                {
                    BTree<T,K> tree = new BTree<T,K>(null, value);
                    rootTree = tree;
                }
                else {
                    rootTree.SetValue(value);
                }
            }
            get {
                return rootTree != null ? rootTree.Value : null;
            }
        }

        private BTree<T,K> currentTree;

        /// <summary>
        /// Add the new Tree node to the main Tree
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="value"></param>
        public void AddNewTree(BProcessUnit<K> parent,BProcessUnit<K> value) {
            ////TODO Add the new Tree after the parent node
            if (parent != null)
                AddNewTree(parent.UnitTagName, value);
            else {
                BTree<T, K> subTree = new BTree<T, K>(null, value);
                rootTree = subTree;
                currentTree = rootTree;
            }
        }

        /// <summary>
        /// Add the new Tree node to the main Tree
        /// </summary>
        /// <param name="unitTagName"></param>
        /// <param name="value"></param>
        public void AddNewTree(string unitTagName, BProcessUnit<K> value) {
            ////TODO Add the new Tree after the parent node by the name
            if (rootTree != null)
            {
                BTree<T, K> result = GetBTressNode(unitTagName, rootTree);
                if (result != null)
                {
                    BTree<T, K> subTree = new BTree<T, K>(result, value);
                    result.SubTrees.Add(subTree);
                }
            }
            else {
                rootTree = new BTree<T, K>(null, value);
                currentTree = rootTree;
            }
        }

        /// <summary>
        /// move the index to the next node
        /// </summary>
        /// <param name="nextNode"></param>
        public void MoveToNext(BProcessUnit<K> nextNode) {
            MoveToNext(nextNode.UnitTagName);
        }

        /// <summary>
        /// move the index to the next node
        /// </summary>
        /// <param name="nextUnitName"></param>
        public void MoveToNext(string nextUnitName) {
            if (string.IsNullOrEmpty(nextUnitName))
            {
                currentTree = null;
            }
            else {
                if (currentTree != null)
                {
                    for (int i = 0; i < currentTree.SubTrees.Count; i++)
                    {
                        if (currentTree.SubTrees[i].Value.UnitTagName == nextUnitName)
                        {
                            currentTree = currentTree.SubTrees[i];
                            break;
                        }
                    }
                }
            }

        }

        /// <summary>
        /// get current unit
        /// </summary>
        /// <returns></returns>
        public BProcessUnit<K> GetCurrentUnit() {
            return currentTree != null ? currentTree.Value : null;
        }


        /// <summary>
        /// set the index to root
        /// </summary>
        public void Reset() {
            currentTree = rootTree;
        }

        /// <summary>
        /// Find node by name
        /// </summary>
        /// <param name="unitName"></param>
        /// <returns></returns>
        public BProcessUnit<K> FindNodeByName(string unitTagName) {
            if (rootTree != null)
            {
                BTree<T,K> result = GetBTressNode(unitTagName, rootTree);
                return result != null ? result.Value : null;
            }
            return null;
        }

        /// <summary>
        /// Find tree node by name
        /// </summary>
        /// <param name="unitTagName"></param>
        /// <returns></returns>
        private BTree<T,K> FindTreeNodeByName(string unitTagName) {
            if (rootTree != null)
            {
                return GetBTressNode(unitTagName,rootTree);
            }
            return null;
        }

        /// <summary>
        /// find the first Tree node
        /// </summary>
        /// <param name="unitTagName"></param>
        /// <param name="tree"></param>
        /// <returns></returns>
        private BTree<T,K> GetBTressNode(string unitTagName, BTree<T,K> tree)
        {
            if (tree.Value.UnitTagName == unitTagName)
            {
                return tree;
            }
            else
            {
                List<BTree<T,K>> SubTrees = tree.SubTrees;
                for (int i = 0; i < SubTrees.Count; i++)
                {
                    BTree <T,K> result = GetBTressNode(unitTagName, SubTrees[i]);
                    if (result != null)
                        return result;
                    else
                        continue;
                }
                return null;
            }
        }

        /// <summary>
        /// Tree Struct
        /// </summary>
        private class BTree<T,K>  where T : BProcessUnit<K> where K : BProcessItem
        {

            private BTree<T,K> parentTree;

            /// <summary>
            ///  subTrees
            /// </summary>
            private List<BTree<T,K>> subTrees = new List<BTree<T,K>>();

            private BProcessUnit<K> value;


            public BTree<T,K> ParentTree {

                get {
                    return parentTree;
                }
            }

            public BProcessUnit<K> Value {

                get {
                    return value;
                }
            }

            public List<BTree<T,K>> SubTrees {
                get {
                    return subTrees;
                }
            }

            public void SetParentTree(BTree<T,K> parent) {
                this.parentTree = parent;
            }

            public void SetValue(BProcessUnit<K> value) {
                this.value = value;
            }

            public void SetSubTree(BTree<T,K> subTree) {
                subTrees.Add(subTree);
            }

            public BTree (BTree<T,K> parent, BProcessUnit<K> value)
            {
                this.parentTree = parent;
                this.value = value;
            }

        }
    }


}

