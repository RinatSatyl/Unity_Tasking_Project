// Класс для контролирования UI экрана списка UI

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tasking
{
    public class TaskUIManager : MonoBehaviour
    {
        [SerializeField] GameObject taskUIPrefab;
        [SerializeField] GameObject taskUIList;

        List<TaskUI> taskUIObjects = new List<TaskUI>();

        public static TaskUIManager Instance;
        // Список цветов огранизованные по статусу задачи
        private Dictionary<TaskManager.TaskingStatus, Color> taskColor = new Dictionary<TaskManager.TaskingStatus, Color>();
        // Публичная read-only ссылка на список цветов
        public Dictionary<TaskManager.TaskingStatus, Color> TaskColor { get { return taskColor; } }

        private void Start()
        {
            Instance = this;

            // Пополнить список цветов
            taskColor.Add(TaskManager.TaskingStatus.COMPLETED, Color.green);
            taskColor.Add(TaskManager.TaskingStatus.IN_PROGRESS, new Color(255, 215, 0));
            taskColor.Add(TaskManager.TaskingStatus.OPEN, Color.gray);
            taskColor.Add(TaskManager.TaskingStatus.REVIEWING, Color.yellow);
        }

        public void OnTaskCreated(TaskManager.TaskingTask newTask)
        {
            GameObject newTaskUI = Instantiate(taskUIPrefab, taskUIList.transform);
            taskUIObjects.Add(newTaskUI.GetComponent<TaskUI>());
            newTaskUI.GetComponent<TaskUI>().SetInformation(newTask);
        }

        public void OnTaskDeleted(TaskManager.TaskingTask newTask)
        {
            foreach(TaskUI thisTaskUI in taskUIObjects)
            {
                if (thisTaskUI.TaskName == newTask.name)
                {
                    Destroy(thisTaskUI.gameObject);
                    taskUIObjects.Remove(thisTaskUI);
                    return;
                }
            }
        }

        public void OnTaskUpdated(TaskManager.TaskingTask newTask)
        {
            foreach (TaskUI thisTaskUI in taskUIObjects)
            {
                if (thisTaskUI.TaskName == newTask.name)
                {
                    thisTaskUI.SetInformation(newTask);
                    return;
                }
            }
        }
    }
}