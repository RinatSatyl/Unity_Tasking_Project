// Класс для контролирования показываемой информации в ячейке в списке задач
// Так же хранит в себе ссылку на задачу

using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Tasking
{
    public class TaskUI : MonoBehaviour
    {
        // ссылки на объекты в префабе
        [SerializeField] TMP_Text taskName;
        [SerializeField] TMP_Text taskAssignee;
        [SerializeField] Image taskStatusBackground;
        [SerializeField] TMP_Text taskDueTime;
        [SerializeField] TMP_Dropdown taskStatusDropdown;

        // Дата окончания задачи
        int dueDay = 0;
        int dueMonth = 0;
        // Id задачи
        string thisTaskId = string.Empty;

        // Публичные ссылки на ID и статус задачи
        public string TaskID { get { return thisTaskId; } }
        public int TaskStatus { get { return taskStatusDropdown.value; } }

        // Метод
        // Извлекает информацию задачи и передаёт её UI элементам
        public void SetInformation(TaskingTask receivedTask)
        {
            // Сохранить id задачи
            thisTaskId = receivedTask.id;

            // Заполнить dropdown опции возможными статусами задачи
            for (int i = 0; i < TaskingManager.Instance.PossibleTaskStatuses; i++)
            {
                taskStatusDropdown.options.Add(new TMP_Dropdown.OptionData(TaskingUIManager.Instance.TaskStatusName[(TaskingStatus)i]));
            }

            // Задать текст название задачи, кому поручено, текущий статус, дату окончания
            taskName.text = receivedTask.name;
            taskAssignee.text = receivedTask.assignee;
            taskStatusDropdown.value = (int)receivedTask.status;
            taskDueTime.text = new DateTime(DateTime.Now.Year, receivedTask.month, receivedTask.day).ToString("MMMM dd");

            // Поставить цвет фона переключателя статуса
            taskStatusBackground.color = TaskingUIManager.Instance.TaskColor[(TaskingStatus)receivedTask.status];

            // Скопировать дату окончания
            dueDay = receivedTask.day;
            dueMonth = receivedTask.month;

            taskStatusDropdown.onValueChanged.AddListener(UpdateStatus);
        }

        // Метод для обновления статуса этой задачи
        public void UpdateStatus(int newState)
        {
            TaskingManager.Instance.UpdateTask(thisTaskId, taskName.text, taskAssignee.text, (TaskingStatus)newState, dueDay, dueMonth);
            // Поставить цвет фона переключателя статуса
            taskStatusBackground.color = TaskingUIManager.Instance.TaskColor[(TaskingStatus)newState];
        }
        public void DeleteTask()
        {
            TaskingManager.Instance.DeleteTask(thisTaskId);
        }

        private void OnDestroy()
        {
            taskStatusDropdown.onValueChanged.RemoveAllListeners();
        }
    }
}