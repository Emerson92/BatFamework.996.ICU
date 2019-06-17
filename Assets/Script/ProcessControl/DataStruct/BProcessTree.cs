using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace THEDARKKNIGHT.ProcessCore.DataStruct.Tree {
    /****************************************
     * 
     *         Tree Data Struct
     *  
     * ************************************/
    public class BProcessTree<T> where T : BProcessItem
    {
        private BTree<T> rootTree;

        /// <summary>
        /// Get the root of tree
        /// </summary>
        public BProcessUnit<T> RootTree {
            set {
                if (rootTree == null)
                {
                    BTree<T> tree = new BTree<T>(null, value);
                    rootTree = tree;
                }
                else {
                    rootTree.SetValue(value);
                }
            }
            get {
                return rootTree.Value;
            }
        }

        private BTree<T> currentTree;

        /// <summary>
        /// Add the new Tree node to the main Tree
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="value"></param>
        public void AddNewTree(BProcessUnit<T> parent,BProcessUnit<T> value) {
            ////TODO Add the new Tree after the parent node
            AddNewTree(parent.UnitTagName, value);
        }

        /// <summary>
        /// Add the new Tree node to the main Tree
        /// </summary>
        /// <param name="unitTagName"></param>
        /// <param name="value"></param>
        public void AddNewTree(string unitTagName, BProcessUnit<T> value) {
            ////TODO Add the new Tree after the parent node by the name
            if (rootTree != null) {
                BTree<T> result = GetBTressNode(unitTagName, rootTree);
                if (result != null) {
                    BTree<T> subTree = new BTree<T>(result, value);
                    result.SubTrees.Add(subTree);
                }
            }
        }

        /// <summary>
        /// move the index to the next node
        /// </summary>
        /// <param name="nextNode"></param>
        public void MoveToNext(BProcessUnit<T> nextNode) {
            MoveToNext(nextNode.UnitTagName);
        }

        /// <summary>
        /// move the index to the next node
        /// </summary>
        /// <param name="nextUnitName"></param>
        public void MoveToNext(string nextUnitName) {
            if (currentTree != null) {
                for (int i = 0 ; i < currentTree.SubTrees.Count  ;i++) {
                    if (currentTree.SubTrees[i].Value.UnitTagName == nextUnitName) {
                        currentTree = currentTree.SubTrees[i];
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// get current unit
        /// </summary>
        /// <returns></returns>
        public BProcessUnit<T> GetCurrentUnit() {
            return currentTree.Value;
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
        public BProcessUnit<T> FindNodeByName(string unitTagName) {
            if (rootTree != null)
            {
                BTree<T> result = GetBTressNode(unitTagName, rootTree);
                return result != null ? result.Value : null;
            }
            return null;
        }

        /// <summary>
        /// Find tree node by name
        /// </summary>
        /// <param name="unitTagName"></param>
        /// <returns></returns>
        private BTree<T> FindTreeNodeByName(string unitTagName) {
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
        private BTree<T> GetBTressNode(string unitTagName, BTree<T> tree)
        {
            if (tree.Value.UnitTagName == unitTagName)
            {
                return tree;
            }
            else
            {
                List<BTree<T>> SubTrees = tree.SubTrees;
                for (int i = 0; i < SubTrees.Count; i++)
                {
                    BTree <T> result = GetBTressNode(unitTagName, SubTrees[i]);
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
        private class BTree<T> where T : BProcessItem
        {

            private BTree<T> parentTree;

            /// <summary>
            ///  subTrees
            /// </summary>
            private List<BTree<T>> subTrees = new List<BTree<T>>();

            private BProcessUnit<T> value;


            public BTree<T> ParentTree {

                get {
                    return parentTree;
                }
            }

            public BProcessUnit<T> Value {

                get {
                    return value;
                }
            }

            public List<BTree<T>> SubTrees {
                get {
                    return subTrees;
                }
            }

            public void SetParentTree(BTree<T> parent) {
                this.parentTree = parent;
            }

            public void SetValue(BProcessUnit<T> value) {
                this.value = value;
            }

            public void SetSubTree(BTree<T> subTree) {
                subTrees.Add(subTree);
            }

            public BTree (BTree<T> parent, BProcessUnit<T> value)
            {
                this.parentTree = parent;
                this.value = value;
            }

        }
    }


}

