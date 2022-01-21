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
    public class TaskingTask
    {
        public string name;
        public string assignee;
        public int status;
        public int day;
        public int month;
        public string id;

        public TaskingTask(string name, string assignee, int status, int day, int month)
        {
            this.name = name;
            this.assignee = assignee;
            this.status = status;
            this.day = day;
            this.month = month;
            id = Guid.NewGuid().ToString();

        }
    }

    public class TaskManager : MonoBehaviour
    {
        // Статус задачи
        [System.Serializable]
        public enum TaskingStatus : int
        {
            OPEN = 0,
            COMPLETED = 1,
            IN_PROGRESS = 2,
            REVIEWING = 3
        }

        // Список задач
        List<TaskingTask> taskList = new List<TaskingTask>();

        // Публичная только для чтения ссылка на список задач
        public List<TaskingTask> TaskList { get { return taskList; } }
        public int PossibleTaskStatuses = 4;

        // Эвенты для обновления UI
        [SerializeField] UnityEvent<TaskingTask> TaskCreated;
        [SerializeField] UnityEvent<string> TaskDeleted;
        [SerializeField] UnityEvent<TaskingTask> TaskUpdated;
        [SerializeField] UnityEvent TaskOnClear;

        // Статичная ссылка на TaskManager, для легкого доступа
        public static TaskManager Instance;

        private void Start()
        {
            Instance = this;
        }

        // Метод добавляющии задачу с указаной информацией в список задач 
        public void CreateTask(string taskName, string taskAssignee, TaskingStatus taskStatus, int day, int month)
        {
            // Создать новый объект задачи
            // Добавит объект в список задач
            CreateTask(new TaskingTask(taskName, taskAssignee, (int)taskStatus, day, month));
        }
        public void CreateTask(TaskingTask newTask)
        {
            // Добавит объект в список задач
            taskList.Add(newTask);
            // Вызвать эвент с ссылкой на объект задачи
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
                    return;
                }
            }
        }
        // Метод для обновления информации задачи
        public void UpdateTask(string taskName, string taskAssignee, TaskingStatus taskStatus, int day, int month)
        {
            // Создать новый объект задачи
            TaskingTask updatedTask = new TaskingTask(taskName, taskAssignee, (int)taskStatus, day, month);

            int count = 0;
            foreach (TaskingTask thisTask in taskList)
            {
                if (thisTask.name == taskName)
                {
                    break;
                }
                count++;
            }
            Debug.Log(count);

            taskList.RemoveAt(count);
            taskList.Insert(count, updatedTask);

            // Вызвать эвент с ссылкой на объект задачи
            TaskUpdated.Invoke(updatedTask);
        }
        // Метод для зачистки списка задач
        public void ClearTaskList()
        {
            taskList.Clear();
            TaskOnClear.Invoke();
        }
    }
}