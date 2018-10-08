using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SuperButton.Models.SataticClaass
{
   
    internal class GuiUpdateQueue
    {
        public  static ConcurrentQueue<byte[]> ParamUpdateQueues = new ConcurrentQueue<byte[]>();
        public static ManualResetEvent queueIsFull = new ManualResetEvent(false);
            //packet size depends on protocol

        private static readonly int maxSize = 10;

        // public GuiUpdateQueue(int maxSize) { this.maxSize = maxSize;}

        public static int GetCount()
        {
            return ParamUpdateQueues.Count;
        }

        public static bool Enqueue(byte[] dataBytes)
        {


            if (ParamUpdateQueues.Count >= maxSize) //whlie
            {
                //TODO create another queue to blocked Tasks
                return false;
                // Monitor.Wait(queue);
            }
            else
            {
                ParamUpdateQueues.Enqueue(dataBytes);
                return true;
            }

            //if (queue.Count == 1)
            //{
            //    // wake up any blocked dequeue
            //    Monitor.PulseAll(queue);
            //}
        }

        public static byte[] Dequeue()
        {
            byte[] data1;
            ParamUpdateQueues.TryDequeue(out data1);
            return data1;
        
        }
        // class SizeQueue<T>
        // {
        // private readonly Queue<T> queue = new Queue<T>();
        //  private readonly int maxSize;
        //  public SizeQueue(int maxSize) { this.maxSize = maxSize; }


        //public T Dequeue()
        //{
        //    lock (queue)
        //    {
        //        while (queue.Count == 0)
        //        {
        //            Monitor.Wait(queue);
        //        }
        //        T item = queue.Dequeue();
        //        if (queue.Count == maxSize - 1)
        //        {
        //            // wake up any blocked enqueue
        //            Monitor.PulseAll(queue);
        //        }
        //        return item;
        //    }
        //}
        //  }
        //}
    }
}