using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace THEDARKKNIGHT.ProcessCore.DataStruct {

    /// <summary>
    ///  Process Item Data
    /// </summary>
    public class ProcessItemData<T,K> where T : BProcessUnit<K> where K : BProcessItem
    {
        /// <summary>
        /// the next process Item data
        /// </summary>
        public ProcessItemData<T, K> NextItem;

        /// <summary>
        /// the last process Item data
        /// </summary>
        public ProcessItemData<T, K> LastItem;

        /// <summary>
        /// the current value
        /// </summary>
        public T Value;
    }

}
