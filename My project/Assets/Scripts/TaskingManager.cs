// Класс содержащий список задач, логика для добавления, удаления задач, обновления информации в задаче.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Tasking
{
    // Стракт задачи с информацией
    [System.Serializable]
    public struct TaskingTask
    {
        public string name; // Название/Заголовок задачи
        public string assignee; // Кому назначена
        public int status; // Статус задачи
        public int day; // День срока окончания
        public int month; // Месяц срока окончания
        public string id; // Уникальный ID задачи

        public TaskingTask(string name, string assignee, int status, int day, int month, string id = "")
        {
            this.name = name;
            this.assignee = assignee;
            this.status = status;
            this.day = day;
            this.month = month;

            if (id != "")
            {
                this.id = id;
            }
            else
            {
                this.id = Guid.NewGuid().ToString();
            }
        }
    }

    // Стракт статуса задачи
    [System.Serializable]
    public enum TaskingStatus : int
    {
        OPEN = 0,
        COMPLETED = 1,
        IN_PROGRESS = 2,
        REVIEWING = 3
    }

    public class TaskingManager : MonoBehaviour
    {
        // Список задач
        List<TaskingTask> taskList = new List<TaskingTask>();

        // Публичная только для чтения ссылка на список задач
        public List<TaskingTask> TaskList { get { return taskList; } }
        // Количество состояний статуса задачи
        public int PossibleTaskStatuses = 4;

        // Эвенты для обновления UI
        [SerializeField] UnityEvent<TaskingTask> TaskCreated;
        [SerializeField] UnityEvent<string> TaskDeleted;
        [SerializeField] UnityEvent<TaskingTask> TaskUpdated;
        [SerializeField] UnityEvent TaskOnClear;

        // Статичная ссылка на TaskManager, для легкого доступа
        public static TaskingManager Instance;

        private void Start()
        {
            Instance = this;
        }

        // Метод для создания задачи с указаной информацией
        public void CreateTask(string taskName, string taskAssignee, TaskingStatus taskStatus, int day, int month)
        {
            // Создать новый объект задачи и добавит его в список задач
            CreateTask(new TaskingTask(taskName, taskAssignee, (int)taskStatus, day, month));
        }
        // Метод для добавления задачи в список задач. 
        public void CreateTask(TaskingTask newTask)
        {
            // Добавит объект в список задач
            taskList.Add(newTask);
            // Вызвать эвент о создании новой задачи с ссылкой на объект задачи
            TaskCreated.Invoke(newTask);
        }
        // Метод для удаления задачи с списка задач
        public void DeleteTask(string taskId)
        {
            foreach (TaskingTask taskInList in taskList)
            {
                if (taskInList.id == taskId)
                {
                    TaskDeleted.Invoke(taskInList.id);
                    taskList.Remove(taskInList);
                    break;
                }
            }
        }
        // Метод для обновления информации задачи
        public void UpdateTask(string thisTaskId, string taskName, string taskAssignee, TaskingStatus taskStatus, int day, int month)
        {
            int count = 0;
            foreach (TaskingTask thisTask in taskList)
            {
                if (thisTask.id == thisTaskId)
                {
                    // Создать новый объект задачи для замены
                    TaskingTask updatedTask = new TaskingTask(taskName, taskAssignee, (int)taskStatus, day, month, thisTaskId);

                    // Удалить задачу со старой информацией
                    taskList.RemoveAt(count);
                    // Вставить в слот удалённой задачи, новую задачу с обновлённой информацией
                    taskList.Insert(count, updatedTask);

                    // Оповестить программу о том что список задач был обновлён
                    TaskUpdated.Invoke(updatedTask);
                    break;
                }
                count++;
            }
        }
        // Метод для зачистки списка задач
        public void ClearTaskList()
        {
            taskList.Clear();
            TaskOnClear.Invoke();
        }
    }
}