// Класс для контролирования UI экрана списка UI

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tasking
{
    public class TaskUIManager : MonoBehaviour
    {
        private Dictionary<TaskManager.TaskingStatus, Color> taskColor = new Dictionary<TaskManager.TaskingStatus, Color>();

        public Dictionary<TaskManager.TaskingStatus, Color> TaskColor { get { return taskColor; } }
    }
}