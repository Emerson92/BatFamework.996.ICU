using System.Collections;
using System.Collections.Generic;
using THEDARKKNIGHT.ProcessCore.DataStruct;
using UnityEngine;
namespace THEDARKKNIGHT.ProcessCore.Interface
{
    public interface IProcessForerunner<T,K> where T : BProcessUnit<K> where K : BProcessItem
    {
        /// <summary>
        /// Get the Length of link
        /// </summary>
        int GetLength();

        /// <summary>
        /// Get Process item
        /// </summary>
        T NextProcessItem();

        /// <summary>
        /// Set Process item
        /// </summary>
        void SetProcessItem(T unit);

        /// <summary>
        /// Remove the item
        /// </summary>
        /// <param name="itemData"></param>
        void RemoveProcessItem(T itemData);

        /// <summary>
        /// Remove the item accroding of the index
        /// </summary>
        /// <param name="index"></param>
        void RemoveProcessItem(int index);

        void ResetIndicatorToStart();

        /// <summary>
        /// get the current process Item
        /// </summary>
        /// <returns></returns>
        T GetCurrentProcessItem();

        /// <summary>
        /// get the process item at index
        /// </summary>
        /// <returns></returns>
        T GetProcessItemAtIndex(int index);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstNode"></param>
        /// <param name="secondNode"></param>
        void ReconnectNode(T firstNode, T secondNode);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstNode"></param>
        /// <param name="secondNode"></param>
        void SwitchPositon(T firstNode, T secondNode);

        /// <summary>
        /// Get the first node
        /// </summary>
        /// <returns></returns>
        T GetFirstNode();

        /// <summary>
        /// get the end node
        /// </summary>
        /// <returns></returns>
        T GetEndNode();

        /// <summary>
        /// Get the first point node
        /// </summary>
        /// <returns></returns>
        ProcessItemData<T, K> GetFirstNodePoint();

        /// <summary>
        /// insert the new node to link
        /// </summary>
        /// <param name="originalNode"></param>
        /// <param name="newNode"></param>
        void InsertNodeToLink(T targetNode, T newNode);

        /// <summary>
        /// insert the link on targetNode
        /// </summary>
        /// <param name="targetNode"></param>
        /// <param name="link"></param>
        void InsertLinkAtNode(T targetNode, IProcessForerunner<T, K> link);

        /// <summary>
        /// Reback all the data
        /// </summary>
        void RebackData();

        /// <summary>
        /// clear all the data
        /// </summary>
        void Destory();
    }



}

