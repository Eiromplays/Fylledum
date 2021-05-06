using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public class AsyncHelper
    {
        /// <summary>
        ///    Schedules a task on a different thread (fire and forget)
        /// </summary>
        /// <param name="name">The name of the task.</param>
        /// <param name="task">The task to run.</param>
        /// <param name="exceptionHandler">The optional exception handler.</param>
        public static void Schedule(string name, Func<Task> task, Action<Exception> exceptionHandler = null)
        {
            Task.Run(async () =>
            {
                try
                {
                    await task();
                }
                catch (Exception e)
                {
                    if (exceptionHandler != null)
                    {
                        exceptionHandler(e);
                    }
                    else
                    {
                        Debug.LogError($"Exception occured in task \"{name}\" \n {e}");
                    }
                }
            });
        }
    }
}
