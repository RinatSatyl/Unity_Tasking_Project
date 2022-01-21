// Класс для контролирования UI экрана списка UI

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tasking
{
    public class TaskUIManager : MonoBehaviour
    {
        [SerializeField] GameObject taskUIPrefab;
        [SerializeField] GameObject taskUIList;
        [SerializeField] RectTransform contentTransform;
        [SerializeField] GridLayoutGroup gridLayoutGroup;

        List<TaskUI> taskUIObjects = new List<TaskUI>();

        public static TaskUIManager Instance;
        // Список цветов огранизованные по статусу задачи
        private Dictionary<TaskManager.TaskingStatus, Color> taskColor = new Dictionary<TaskManager.TaskingStatus, Color>();
        // Список читаемого названия статуса задачи
        private Dictionary<TaskManager.TaskingStatus, string> taskStatusName = new Dictionary<TaskManager.TaskingStatus, string>();

        // Публичная read-only ссылка на список цветов
        public Dictionary<TaskManager.TaskingStatus, Color> TaskColor { get { return taskColor; } }
        // Публичная read-only список названий для статусов
        public Dictionary<TaskManager.TaskingStatus, string> TaskStatusName { get { return taskStatusName; } }

        private void Start()
        {
            Instance = this;

            // Пополнить какой цвет ставит в зависимости от статуса
            taskColor.Add(TaskManager.TaskingStatus.COMPLETED, Color.green);
            taskColor.Add(TaskManager.TaskingStatus.IN_PROGRESS, new Color(0.9f, 0.47f, 0.25f) );
            taskColor.Add(TaskManager.TaskingStatus.OPEN, Color.white);
            taskColor.Add(TaskManager.TaskingStatus.REVIEWING, Color.yellow);
            // Заполнить читаемое название статуса
            taskStatusName.Add(TaskManager.TaskingStatus.COMPLETED, "Закончено");
            taskStatusName.Add(TaskManager.TaskingStatus.IN_PROGRESS, "В прогрессе");
            taskStatusName.Add(TaskManager.TaskingStatus.OPEN, "Открыто");
            taskStatusName.Add(TaskManager.TaskingStatus.REVIEWING, "Рассматривается");
        }

        public void OnTaskCreated(TaskManager.TaskingTask newTask)
        {
            GameObject newTaskUI = Instantiate(taskUIPrefab, taskUIList.transform);
            taskUIObjects.Add(newTaskUI.GetComponent<TaskUI>());
            newTaskUI.GetComponent<TaskUI>().SetInformation(newTask);
            UpdateScrollAreaSize();
        }

        public void OnTaskDeleted(TaskManager.TaskingTask newTask)
        {
            foreach(TaskUI thisTaskUI in taskUIObjects)
            {
                if (thisTaskUI.TaskName == newTask.name)
                {
                    Destroy(thisTaskUI.gameObject);
                    taskUIObjects.Remove(thisTaskUI);
                    UpdateScrollAreaSize();
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

        void UpdateScrollAreaSize()
        {
            contentTransform.sizeDelta = new Vector2(gridLayoutGroup.cellSize.x, gridLayoutGroup.cellSize.y * taskUIObjects.Count);
        }
    }
}