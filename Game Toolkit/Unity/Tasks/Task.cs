using System;
using UnityEngine;
using Voxels.Objects;

namespace System.Threading.Tasks
{
    /// <summary>
    /// Base class for tasks
    /// </summary>
    public class Task
    {
        public static TaskFactory Factory { get { return TaskFactory.Instance; } }
        public DateTime ExecutionTime { get; set; }
        public bool IsCompleted { get; protected set; }

		Action _expr;

        public Task()
        {
            ExecutionTime = DateTime.Now;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expr"></param>
        public Task(Action expr)
        {
            _expr = expr;
            ExecutionTime = DateTime.Now;
        }

        /// <summary>
        /// Runs this task setting the result.
        /// </summary>
        /// <returns></returns>
        public virtual void Execute()
        {
            if (_expr != null)
                _expr();

			IsCompleted = true;
        }
    }

    /// <summary>
    /// Temp type def until mono version gets updated
    /// </summary>
    public class Task<T> : Task
    {
        Func<T> _expr;

        public T Result { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expr"></param>
        public Task(Func<T> expr)
        {
            _expr = expr;
            ExecutionTime = DateTime.Now;
        }

        /// <summary>
        /// Runs this task setting the result.
        /// </summary>
        /// <returns></returns>
        public override void Execute()
        {
			if (_expr != null)
            	Result = _expr();

			IsCompleted = true;
        }
    }
}