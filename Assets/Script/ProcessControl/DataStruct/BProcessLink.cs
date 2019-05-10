using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.ProcessCore.Interface;
using UnityEngine;
namespace THEDARKKNIGHT.ProcessCore.DataStruct
{
    /// <summary>
    /// The Link that manage all the task struct
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    public class BProcessLink<T, K> : IProcessForerunner<T, K> where T : BProcessUnit<K> where K : BProcessItem
    {
        /// <summary>
        /// data list
        /// </summary>
        private List<ProcessItemData<T, K>> processItemList = new List<ProcessItemData<T, K>>();

        /// <summary>
        /// The current item data
        /// </summary>
        private ProcessItemData<T, K> currentItemData;

        /// <summary>
        /// get the arrayList num
        /// </summary>
        /// <returns></returns>
        public int GetLength()
        {
            int count = 0;
            ProcessItemData<T, K> tempItem = currentItemData;
            while (tempItem.LastItem != null) {
                tempItem = tempItem.LastItem;
                count++;
            }
            tempItem = currentItemData;
            while (tempItem.NextItem != null) {
                tempItem = tempItem.NextItem;
                count++;
            }
            return count+1;
        }

        /// <summary>
        /// The index move to the next
        /// </summary>
        public void NextProcessItem()
        {
            currentItemData = currentItemData.NextItem;
        }

        /// <summary>
        ///  remove the proccess item
        /// </summary>
        /// <param name="itemData"></param>
        public void RemoveProcessItem(T itemData)
        {
            ProcessItemData<T, K> targetItem = null;
            SeachProcessItem(itemData, out targetItem);
            if (targetItem !=null)
                ReconnectNode(targetItem.LastItem, targetItem.NextItem);
        }


        /// <summary>
        /// remove the item according to i
        /// </summary>
        /// <param name="index"></param>
        public void RemoveProcessItem(int index)
        {
            if (index < 0 && index > GetLength())
                return;
            ProcessItemData<T, K> tempItem = GetIndexProcessItem(index);
            ReconnectNode(tempItem.LastItem, tempItem.NextItem);
        }

        private ProcessItemData<T, K> GetIndexProcessItem(int index)
        {
            ProcessItemData<T, K> tempItem = currentItemData;
            while (tempItem.LastItem != null)
            {
                tempItem = tempItem.LastItem;
            }
            int num = 0;
            while (tempItem.NextItem != null)
            {
                if (num < index - 1)
                    num++;
                else
                    break;
            }

            return tempItem;
        }

        /// <summary>
        /// Add the new node to the Link
        /// </summary>
        /// <param name="itemData"></param>
        public void SetProcessItem(T data)
        {
            if (data != null)
            {
                ProcessItemData<T, K> item = new ProcessItemData<T,K>();
                item.Value = data;
                currentItemData.NextItem = item;
                item.LastItem = currentItemData;
                currentItemData = item;
                processItemList.Add(currentItemData);
            }
        }

        /// <summary>
        /// return the current node
        /// </summary>
        /// <returns>current node</returns>
        public T  GetCurrentProcessItem()
        {
            return currentItemData.Value;
        }

        /// <summary>
        /// Reconnecd two node
        /// </summary>
        /// <param name="firstNode"></param>
        /// <param name="secondNode"></param>
        public void ReconnectNode(T firstNode, T secondNode)
        {
            ProcessItemData<T, K> firstItem,secondItem = null;
            SeachProcessItem(firstNode  , out firstItem );
            SeachProcessItem(secondNode , out secondItem );
            if (firstItem != null && secondItem != null)
            {
                firstItem.NextItem = secondItem;
                secondItem.LastItem = firstItem;
            }
        }

        private void ReconnectNode(ProcessItemData<T, K> firstItem, ProcessItemData<T, K>  secondItem) {
            firstItem.NextItem = secondItem;
            secondItem.LastItem = firstItem;
        }

        private void SeachProcessItem(T node, out ProcessItemData<T, K> targetItem)
        {
            targetItem = null;
            ProcessItemData<T, K> tempItem = currentItemData;
            while (tempItem.LastItem != null)
            {

                if (tempItem.LastItem.Value.Equals(node))
                {
                    targetItem = tempItem;
                    break;
                }
                tempItem = tempItem.LastItem;
            }
            tempItem = currentItemData;
            while (tempItem.NextItem != null)
            {
                if (tempItem.LastItem.Value.Equals(node))
                {
                    targetItem = tempItem;
                    break;
                }
                tempItem = tempItem.NextItem;
            }
        }

        /// <summary>
        /// get the index at index
        /// </summary>
        /// <returns></returns>
        public T GetProcessItemAtIndex(int index)
        {
            ProcessItemData<T,K> temp = GetIndexProcessItem(index);
            return temp.Value;
        }

        /// <summary>
        /// swtich both node position
        /// </summary>
        /// <param name="firstNode"></param>
        /// <param name="secondNode"></param>
        public void SwitchPositon(T firstNode, T secondNode)
        {
            ProcessItemData<T, K> firstItem, secondItem,tempItem = null;
            SeachProcessItem(firstNode, out firstItem );
            SeachProcessItem(secondNode, out secondItem );
            if (firstItem != null && secondItem != null)
            {
                tempItem = firstItem.NextItem;
                firstItem.NextItem = secondItem.NextItem;
                secondItem.NextItem = tempItem;

                tempItem = firstItem.LastItem;
                firstItem.LastItem = secondItem.LastItem;
                secondItem.LastItem = tempItem;
            }
        }

        /// <summary>
        /// get first node
        /// </summary>
        /// <returns></returns>
        public T GetFirstNode()
        {
            return GetIndexProcessItem(1).Value;
        }

        /// <summary>
        /// get end node
        /// </summary>
        /// <returns></returns>
        public T GetEndNode()
        {
            ProcessItemData<T, K> tempItem = currentItemData;
            while ( tempItem.NextItem != null) {
                tempItem = tempItem.NextItem;
            }
            return tempItem.Value;
        }

        /// <summary>
        /// insert the new node to the link,after the targetnode 
        /// </summary>
        /// <param name="originalNode"></param>
        /// <param name="newNode"></param>
        public void InsertNodeToLink(T targetNode, T newNode)
        {
            ProcessItemData<T, K> targetItem,tempItem = null;
            SeachProcessItem( targetNode , out targetItem );
            if (targetItem != null) {
                tempItem = targetItem.NextItem;
                ProcessItemData<T, K> newItem = new ProcessItemData<T, K>();
                newItem.Value = newNode;
                targetItem.NextItem = newItem;
                newItem.LastItem = targetItem;
                newItem.NextItem = tempItem;
            }
        }

        /// <summary>
        /// Recover all the init data
        /// </summary>
        public void RebackData()
        {
            for (int i = 1 ; i < processItemList.Count ; i++ ) {
                processItemList[i-1] = processItemList[i].LastItem;
                processItemList[i - 1].NextItem = processItemList[i];
            }
            processItemList[0].LastItem = null;
            processItemList[processItemList.Count - 1] = null;
            currentItemData = processItemList[0];
        }

        /// <summary>
        /// Destory the data;
        /// </summary>
        public void Destory()
        {
            currentItemData = null;
            processItemList.Clear();
            processItemList = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetNode"></param>
        /// <param name="link"></param>
        public void InsertLinkAtNode(T targetNode, IProcessForerunner<T, K> link)
        {
            /////
            /////TODO finish the method
            ///// 
            ProcessItemData<T, K> targetItem = null;
            SeachProcessItem(targetNode, out targetItem);
            if (targetItem !=null) {
                // = link.GetFirstNode();
                //targetItem.NextItem =
                while (link.GetCurrentProcessItem() != null)
                {
                    T tempNode = link.GetCurrentProcessItem();
                    ProcessItemData<T, K> item = new ProcessItemData<T, K>();
                    link.NextProcessItem();
                }
            }
   
        }
    }
}

